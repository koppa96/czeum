﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace Czeum.Client.Interfaces {
    public interface IDialogService {
        IAsyncOperation<ContentDialogResult> ShowConfirmation(string message);
    }
}
