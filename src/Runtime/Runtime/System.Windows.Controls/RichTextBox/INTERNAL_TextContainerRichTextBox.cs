
/*===================================================================================
* 
*   Copyright (c) Userware/OpenSilver.net
*      
*   This file is part of the OpenSilver Runtime (https://opensilver.net), which is
*   licensed under the MIT license: https://opensource.org/licenses/MIT
*   
*   As stated in the MIT license, "the above copyright notice and this permission
*   notice shall be included in all copies or substantial portions of the Software."
*  
\*====================================================================================*/

#if MIGRATION
using OpenSilver.Internal.Controls;
using System.Collections.Generic;
using System.Windows.Documents;
#else
using Windows.UI.Xaml.Documents;
#endif

#if MIGRATION
namespace System.Windows.Controls
#else
namespace Windows.UI.Xaml.Controls
#endif
{
    internal sealed class INTERNAL_TextContainerRichTextBox : INTERNAL_TextContainer
    {
        private readonly RichTextBox _parent;

        public INTERNAL_TextContainerRichTextBox(RichTextBox parent)
            : base(parent)
        {
            _parent = parent;
        }

        public override string Text => _parent.GetRawText();

        protected override void OnTextAddedOverride(TextElement textElement)
        {
            if (textElement is Paragraph paragraph)
            {
                var ops = new List<QuillDelta>();

                foreach (var inline in paragraph.Inlines)
                {
                    if (inline is Run run)
                    {
                        var op = new QuillDelta();
                        op.Insert = run.Text;
                        ops.Add(op);
                    }
                    else if(inline is Hyperlink link)
                    {
                        var op = new QuillDelta();
                        string linkText = "";
                        foreach(var linkInline in link.Inlines)
                        {
                            if(linkInline is Run linkRun)
                            {
                                linkText += linkRun.Text;
                            }
                        }

                        op.Insert = new
                        {
                            hyperlink = new
                            {
                                text = linkText,
                                href = link.NavigateUri.ToString()
                            }
                        };
                        ops.Add(op);
                    }
                    //TODO: support other Inlines
                }

                var delta = new QuillDeltas();
                delta.Operations = ops.ToArray();
                _parent.InsertDelta(delta);
            }
            else if (textElement is Section)
            {
                //Does not support now
            }
        }

        protected override void OnTextRemovedOverride(TextElement textElement)
        {
            //TODO: implement
        }
    }
}
