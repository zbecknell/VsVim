﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Vim.UI.Wpf.Implementation.CommandMargin;
using Vim.UnitTest;
using Vim.UnitTest.Exports;
using Xunit;

namespace Vim.UI.Wpf.UnitTest
{
    public class CommandMarginControllerIntegrationTest : VimTestBase
    {
        private readonly IVimBuffer _vimBuffer;
        private readonly CommandMarginController _controller;
        private readonly CommandMarginControl _control;
        private readonly TestableClipboardDevice _clipboardDevice;

        public CommandMarginControllerIntegrationTest()
        {
            _vimBuffer = CreateVimBuffer();
            _clipboardDevice = (TestableClipboardDevice)CompositionContainer.GetExportedValue<IClipboardDevice>();

            var parentElement = new FrameworkElement();
            _control = new CommandMarginControl();
            _controller = new CommandMarginController(
                _vimBuffer,
                parentElement,
                _control,
                VimEditorHost.EditorFormatMapService.GetEditorFormatMap(_vimBuffer.TextView),
                VimEditorHost.ClassificationFormatMapService.GetClassificationFormatMap(_vimBuffer.TextView),
                CommonOperationsFactory.GetCommonOperations(_vimBuffer.VimBufferData),
                _clipboardDevice);
        }

        [WpfFact]
        public void QuitDirtyBuffer()
        {
            VimHost.IsDirtyFunc = _ => true;
            _vimBuffer.ProcessNotation(":q<CR>");
            Assert.Equal(Resources.Common_NoWriteSinceLastChange, _control.CommandLineTextBox.Text);
        }
    }
}
