using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using LeakBlocker.AdminView.Desktop.Generated;

namespace LeakBlocker.AdminView.Desktop
{
    internal sealed class InactivePictureEffect : ShaderEffect
    {
        private static readonly PixelShader pixelShader = InitializeShader();

        public static readonly DependencyProperty InputProperty = RegisterPixelShaderSamplerProperty("Input", typeof(InactivePictureEffect), 0);

        public InactivePictureEffect()
        {
            PixelShader = pixelShader;

            UpdateShaderValue(InputProperty);
        }

        private static PixelShader InitializeShader()
        {
            var result = new PixelShader();
            result.SetStreamSource(new MemoryStream(Shaders.InactiveImage));
            return result;
        }
    }
}
