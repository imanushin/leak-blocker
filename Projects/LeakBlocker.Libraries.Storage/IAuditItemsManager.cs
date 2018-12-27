using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Audit;

namespace LeakBlocker.Libraries.Storage
{
    /// <summary>
    /// �������� �� ������ � ������� � ����
    /// </summary>
    public interface IAuditItemsManager
    {
        /// <summary>
        /// ��������� ����� AuditItem'�� � ����
        /// </summary>
        void AddItems(IEnumerable<AuditItem> items);

        /// <summary>
        /// ������ ������ <see cref="AddItems"/>, �� ����������� ������ ���� �������
        /// </summary>
        void AddItem(AuditItem item);

        /// <summary>
        /// ������ ������ �� ���� � ������������ � ��������. �������� �������� ������ <paramref name="topCount"/> ���������, ���������� ���� �� ������ event'�� � �����
        /// ���� <paramref name="topCount"/> == -1, �� ����� ���������� ��� ������
        /// </summary>
        ReadOnlyList<AuditItem> GetItems(AuditFilter filter, int topCount);

        /// <summary>
        /// ������ ������ �� ���� � ������������ � �������� ��� ������. ������ ��� ����������, ���������� ���� �� ������ event'�� � �����
        /// </summary>
        ReadOnlyList<AuditItem> GetItems(ReportFilter filter, Time startTime, Time endTime);
    }
}