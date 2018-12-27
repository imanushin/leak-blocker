using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;

namespace LeakBlocker.Libraries.SystemTools.Devices.Management
{
    internal static class IdentifierDataProvider
    {
        private static readonly string[] hardwareIdentifierPatterns = new[]
        {
            @"HID\\VID_(?<Vendor>.{4})&PID_(?<Product>.{4})&REV_(?<Revision>.{4})&MI_(?<UsbInterface>.{2})&Col(?<CollectionNumber>.{2})",
            @"HID\\VID_(?<Vendor>.{4})&PID_(?<Product>.{4})&REV_(?<Revision>.{4})&MI_(?<UsbInterface>.{2})",
            @"HID\\VID_(?<Vendor>.{4})&PID_(?<Product>.{4})&REV_(?<Revision>.{4})&Col(?<CollectionNumber>.{2})",
            @"HID\\VID_(?<Vendor>.{4})&PID_(?<Product>.{4})&REV_(?<Revision>.{4})",

            @"IDE\\(?<ScsiType>Disk|Sequential|Printer|Processor|Worm|CdRom|Scanner|Optical|Changer|Net|Other)(?<Model>.{40})(?<Revision>.{8})",

            @"1394\\(?<Vendor>.*)&(?<Product>.*)",

            @"ISAPNP\\(?<Vendor>.{3})(?<Product>.{4})_DEV(?<FunctionIndex>.{4})",
            @"ISAPNP\\(?<Vendor>.{3})(?<Product>.{4})",

            @"PCI\\VEN_(?<Vendor>.{4})&DEV_(?<Product>.{4})&SUBSYS_(?<Product>.{4})(?<SigSubSystem>.{4})&REV_(?<Revision>.{2})",
            @"PCI\\VEN_(?<Vendor>.{4})&DEV_(?<Product>.{4})&REV_(?<Revision>.{2})",

            @"PCMCIA\\MTD-(?<MemoryType>.{4})",
            @"PCMCIA\\UNKNOWN_MANUFACTURER-DEV(?<ChildFunctionNumber>.{4})-(?<Checksum>.{4})",
            @"PCMCIA\\UNKNOWN_MANUFACTURER-(?<Checksum>.{4})",       
            @"PCMCIA\\(?<Vendor>.{0,64})-(?<Product>.{0,64})-DEV(?<ChildFunctionNumber>.{4})-(?<Checksum>.{4})",
            @"PCMCIA\\(?<Vendor>.{0,64})-(?<Product>.{0,64})-(?<Checksum>.{4})",

            @"SBP2\\(?<Vendor>.*)&(?<Product>.*)&LUN(?<LogicalUnitNumber>.*)",
            @"SBP2\\(?<Vendor>.*)&(?<Product>.*)&CmdSetId(?<CommandSetId>.*)",

            @"SCSI\\(?<ScsiType>Disk|Sequential|Printer|Processor|Worm|CdRom|Scanner|Optical|Changer|Net|ASCIT8|ASCIT8|Array|Enclosure|RBC|CardReader|Bridge|Other)(?<Vendor>.{8})(?<Product>.{16})(?<Revision>.{4})",

            @"SD\\VID_(?<SdVendor>.{2})&OID_(?<Vendor>.{4})&PID_(?<Product>.{0,5})&REV_(?<Revision>.{3})",
            @"SD\\VID_(?<SdVendor>.{2})&OID_(?<Vendor>.{4})&PID_(?<Product>.{0,5})",
            @"SD\\VID_(?<Vendor>.{4})&PID_(?<Product>.{4})",

            @"USB\\VID_(?<Vendor>.{4})&PID_(?<Product>.{4})&REV_(?<Revision>.{4})&MI_(?<UsbInterface>.{2})",
            @"USB\\VID_(?<Vendor>.{4})&PID_(?<Product>.{4})&REV_(?<Revision>.{4})",
            @"USB\\ROOT_HUB&VID(?<Vendor>.{4})&PID(?<Product>.{4})&REV(?<Revision>.{4})",
            @"USB\\ROOT_HUB20&VID(?<Vendor>.{4})&PID(?<Product>.{4})&REV(?<Revision>.{4})",

            @"USBPRINT\\(?<Model>.{0,20})(?<Checksum>.{4})",

            @"USBSTOR\\(?<ScsiType>Disk|SFloppy|Sequential|Worm|CdRom|Optical|Changer|Other)(?<Vendor>.{8})(?<Product>.{16})(?<Revision>.{4})",

            @"[^\\]*\\PNP(?<Model>.{4})",
            @"[^\\]*\\(?<Unknown001>.*)",
            @"\*PNP(?<Model>.{4})",
            @"\*(?<Unknown002>.*)",
            @"(?<Unknown003>.*)" 
        };

        private static readonly string[] deviceInstanceIdentifierPatterns = new[]
        {
            @"ROOT\\(?<Unknown004>.*)\\(?<Unknown005>.*)",
            @".*\\.*\\(?<SerialNumber>.*)"
        };

        private const string parentSectionName = "ParentIdentifier";
        private const string classSectionName = "Class";

        private const string sectionFormat = "{{{0}:{1}}}";
        private const string separator = "\\";
        private const string identifierFormat = "{{{0}}}";

        private static readonly ReadOnlySet<Regex> hardwareIdentifierExpressions = hardwareIdentifierPatterns.Select(
            currentPattern => new Regex(currentPattern, RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture)).ToReadOnlySet();
        private static readonly ReadOnlySet<Regex> deviceInstanceIdentifierExpressions = deviceInstanceIdentifierPatterns.Select(
            currentPattern => new Regex(currentPattern, RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture)).ToReadOnlySet();

        internal static string CreateIdentifier(string hardwareIdentifier, string deviceInstanceIdentifier, Guid classIdentifier, bool uniqueIdentifierCapability, string parentIdentifier = null)
        {
            var identifierSections = new Dictionary<string, string>();

            if (!string.IsNullOrWhiteSpace(hardwareIdentifier))
                identifierSections.MergeWith(ParseIdentifier(hardwareIdentifier, hardwareIdentifierExpressions), true);

            if ((identifierSections.Count == 0) || uniqueIdentifierCapability)
                identifierSections.MergeWith(ParseIdentifier(deviceInstanceIdentifier, deviceInstanceIdentifierExpressions), true);

            identifierSections[classSectionName] = classIdentifier.ToString();
            identifierSections[parentSectionName] = parentIdentifier ?? string.Empty;

            List<string> formattedSections = identifierSections.Select(currentItem => sectionFormat.Combine(currentItem.Key, currentItem.Value)).ToList();

            formattedSections.Sort();

            return identifierFormat.Combine(string.Join(separator, formattedSections));
        }

        private static Dictionary<string, string> ParseIdentifier(string identifier, ReadOnlySet<Regex> expressions)
        {
            var result = new Dictionary<string, string>();

            foreach (Regex currentExpression in expressions)
            {
                Match match = currentExpression.Match(identifier);
                if (!match.Success)
                    continue;

                foreach (string currentGroupName in currentExpression.GetGroupNames().Where(currentGroupName => !currentGroupName.Equals("0")))
                {
                    if (string.IsNullOrWhiteSpace(currentGroupName))
                    {
                        Log.Write("Wrong group name");
                        continue;
                    }

                    if (!match.Groups[currentGroupName].Success)
                    {
                        Log.Write("Match is unsuccessful for group {0}.", currentGroupName);
                        continue;
                    }

                    if (result.ContainsKey(currentGroupName))
                    {
                        Log.Write("Key {0} was already added.", currentGroupName);
                        continue;
                    }

                    result.Add(currentGroupName, match.Groups[currentGroupName].Value);
                }

                break;
            }

            if (result.Count == 0)
            {
                var debugTemplates = new StringBuilder();
                foreach (Regex currentExpression in expressions)
                    debugTemplates.Append(currentExpression + ", ");

                string patternsList = debugTemplates.ToString();
                if (patternsList.Length > 2)
                    patternsList = patternsList.Substring(0, patternsList.Length - 2);

                Log.Write("Identifier {0} does not match any of the templates ({1}).", identifier, patternsList);
            }

            return result;
        }
    }
}
