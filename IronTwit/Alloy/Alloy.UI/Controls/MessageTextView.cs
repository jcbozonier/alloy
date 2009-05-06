﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Unite.UI.Controls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Unite.UI.Views"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Unite.UI.Views;assembly=Unite.UI.Views"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:MessageTextView/>
    ///
    /// </summary>
    [TemplatePart(Type=typeof(TextBlock), Name=NAME_PART_MESSAGE_TEXT)]
    public class MessageTextView : ContentControl
    {
        private const string NAME_PART_MESSAGE_TEXT = "PART_MessageText";

        protected TextBlock MessageText;

        static MessageTextView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MessageTextView), new FrameworkPropertyMetadata(typeof(MessageTextView)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            MessageText = (TextBlock) GetTemplateChild(NAME_PART_MESSAGE_TEXT);

            SetMessageText((string) Content);
        }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);

            if (MessageText == null)
                return;

            SetMessageText((string) Content);
        }

        private List<Hyperlink> _hyperlinks = new List<Hyperlink>();
        private void SetMessageText(string messageText)
        {
            ClearHyperlinks();
            MessageText.Inlines.Clear();

            if (messageText == null)
                messageText = "";

            var inlineUris = Utilities.InlineUris.Get(messageText);
            var charIndex = 0;
            foreach (var inlineUri in inlineUris)
            {
                if (inlineUri.StartIndex > charIndex)
                {
                    //add text before link
                    var run = new Run(messageText.Substring(charIndex, inlineUri.StartIndex - charIndex));
                    MessageText.Inlines.Add(run);
                    charIndex += run.Text.Length;
                }

                //add Hyperlink
                var hyperlink = new Hyperlink();
                var displayRun = new Run(messageText.Substring(inlineUri.StartIndex, inlineUri.Length));
                hyperlink.Inlines.Add(displayRun);
                // #TODO: Add test... Was failing due to this tweet: "RT @bhalchander,@adnanmahmud: looking for folks to help us test our beta site www.jolkona.org pls RT"
                // hyperlink.NavigateUri = new Uri(displayRun.Text);
                hyperlink.NavigateUri = inlineUri; 
                ListenToHyperlink(hyperlink, true);
                MessageText.Inlines.Add(hyperlink);
                charIndex += inlineUri.Length;
            }

            if (charIndex < messageText.Length)
            {
                //add text at end
                var run = new Run(messageText.Substring(charIndex));
                MessageText.Inlines.Add(run);
            }
        }

        private void ClearHyperlinks()
        {
            foreach (var hyperlink in _hyperlinks)
            {
                ListenToHyperlink(hyperlink, false);
            }
        }

        private void ListenToHyperlink(Hyperlink hyperlink, bool listen)
        {
            if (hyperlink == null)
                throw new ArgumentNullException("hyperlink");

            if (listen)
            {
                hyperlink.Click += hyperlink_Click;
            }
            else
            {
                hyperlink.Click -= hyperlink_Click;
            }
        }

        void hyperlink_Click(object sender, RoutedEventArgs e)
        {
            var hyperlink = (Hyperlink) sender;
            Process.Start(hyperlink.NavigateUri.ToString());
        }
    }
}
