using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Mobile.TriggerActions
{
    public class EntryFrameBackgroundColorAction : TriggerAction<Entry>
    {
        protected override void Invoke(Entry sender)
        {
            if (sender.Parent is Frame frame)
            {
                frame.BackgroundColor = sender.BackgroundColor;
            }
            else
            {
                if (sender.Parent is StackLayout sl)
                {
                    if (sl.Parent is Frame frame2)
                    {
                        frame2.BackgroundColor = sender.BackgroundColor;
                    }
                }
            }
        }
    }
}
