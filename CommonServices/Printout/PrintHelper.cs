using CommonServices.Logger;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Graphics.Printing;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Printing;

namespace CommonServices.Printout
{
    public class PrintHelper
    {
        private readonly double _applicationContentMarginLeft = 0.075;
        private readonly double _applicationContentMarginTop = 0.03;
        private PrintDocument _printDocument;
        private IPrintDocumentSource _printDocumentSource;
        private List<UIElement> _printPreviewPages;
        private event EventHandler _previewPagesCreated;
        private FrameworkElement _firstPage;
        private Page _scenarioPage;

        public PrintHelper(Page scenarioPage)
        {
            this._scenarioPage = scenarioPage;
            _printPreviewPages = new List<UIElement>();
        }

        public virtual void RegisterForPrinting()
        {
            _printDocument = new PrintDocument();
            _printDocumentSource = _printDocument.DocumentSource;
            _printDocument.Paginate += CreatePrintPreviewPages;
            _printDocument.GetPreviewPage += GetPrintPreviewPage;
            _printDocument.AddPages += AddPrintPages;

            PrintManager printManager = PrintManager.GetForCurrentView();
            printManager.PrintTaskRequested += PrintTaskRequested;
        }

        public async Task ShowPrintUIAsync()
        {
            try
            {
                await PrintManager.ShowPrintUIAsync();
            }
            catch (Exception ex)
            {
                FileLogger.Logger.Info("Failed to open printout UI " + ex.Message);
            }
        }

        public virtual void PreparePrintContent(Page page)
        {
            if (_firstPage == null)
            {
                _firstPage = page;
                StackPanel header = (StackPanel)_firstPage.FindName("Header");
                if (header != null)
                    header.Visibility = Visibility.Visible;
            }

            PrintCanvas.Children.Add(_firstPage);
            PrintCanvas.InvalidateMeasure();
            PrintCanvas.UpdateLayout();
        }

        protected virtual void PrintTaskRequested(PrintManager sender, PrintTaskRequestedEventArgs e)
        {
            PrintTask printTask = null;
            printTask = e.Request.CreatePrintTask("Banquet details", sourceRequested =>
            {
                printTask.Completed += async (s, args) =>
                {
                    if (args.Completion == PrintTaskCompletion.Failed)
                    {
                        await _scenarioPage.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
                            FileLogger.Logger.Info("Failed to print");
                        });
                    }
                };

                sourceRequested.SetSource(_printDocumentSource);
            });
        }

        protected virtual void CreatePrintPreviewPages(object sender, PaginateEventArgs e)
        {
            lock (_printPreviewPages)
            {
                _printPreviewPages.Clear();
                PrintCanvas.Children.Clear();

                RichTextBlockOverflow lastRTBOOnPage;
                PrintTaskOptions printingOptions = e.PrintTaskOptions;
                PrintPageDescription pageDescription = printingOptions.GetPageDescription(0);

                lastRTBOOnPage = AddOnePrintPreviewPage(null, pageDescription);

                while (lastRTBOOnPage.HasOverflowContent && lastRTBOOnPage.Visibility == Visibility.Visible)
                    lastRTBOOnPage = AddOnePrintPreviewPage(lastRTBOOnPage, pageDescription);

                if (_previewPagesCreated != null)
                    _previewPagesCreated.Invoke(_printPreviewPages, null);

                PrintDocument printDoc = (PrintDocument)sender;
                printDoc.SetPreviewPageCount(_printPreviewPages.Count, PreviewPageCountType.Intermediate);
            }
        }

        protected virtual void GetPrintPreviewPage(object sender, GetPreviewPageEventArgs e)
        {
            PrintDocument printDoc = (PrintDocument)sender;
            printDoc.SetPreviewPage(e.PageNumber, _printPreviewPages[e.PageNumber - 1]);
        }

        protected virtual void AddPrintPages(object sender, AddPagesEventArgs e)
        {
            for (int i = 0; i < _printPreviewPages.Count; i++)
                _printDocument.AddPage(_printPreviewPages[i]);

            PrintDocument printDoc = (PrintDocument)sender;

            printDoc.AddPagesComplete();
            PrintDone?.Invoke(this, true);
        }

        public virtual void UnregisterForPrinting()
        {
            if (_printDocument == null)
                return;

            _printDocument.Paginate -= CreatePrintPreviewPages;
            _printDocument.GetPreviewPage -= GetPrintPreviewPage;
            _printDocument.AddPages -= AddPrintPages;
            _printDocument = null;

            PrintManager printMan = PrintManager.GetForCurrentView();
            printMan.PrintTaskRequested -= PrintTaskRequested;

            PrintCanvas.Children.Clear();
        }

        protected virtual RichTextBlockOverflow AddOnePrintPreviewPage(RichTextBlockOverflow lastRTBOAdded, PrintPageDescription printPageDescription)
        {
            FrameworkElement page;
            RichTextBlockOverflow textLink;

            if (lastRTBOAdded == null)
            {
                page = _firstPage;

                StackPanel footer = (StackPanel)page.FindName("Footer");
                footer.Visibility = Visibility.Collapsed;
            }
            else
            {
                page = new ContinuationPage(lastRTBOAdded);
            }

            page.Width = printPageDescription.PageSize.Width;
            page.Height = printPageDescription.PageSize.Height;

            Grid printableArea = (Grid)page.FindName("PrintableArea");

            double marginWidth = Math.Max(printPageDescription.PageSize.Width - printPageDescription.ImageableRect.Width, printPageDescription.PageSize.Width * _applicationContentMarginLeft * 2);
            double marginHeight = Math.Max(printPageDescription.PageSize.Height - printPageDescription.ImageableRect.Height, printPageDescription.PageSize.Height * _applicationContentMarginTop * 2);

            printableArea.Width = _firstPage.Width - marginWidth;
            printableArea.Height = _firstPage.Height - marginHeight;

            PrintCanvas.Children.Add(page);
            PrintCanvas.InvalidateMeasure();
            PrintCanvas.UpdateLayout();

            textLink = (RichTextBlockOverflow)page.FindName("ContinuationPageLinkedContainer");

            if (!textLink.HasOverflowContent && textLink.Visibility == Visibility.Visible)
            {
                StackPanel footer = (StackPanel)page.FindName("Footer");
                footer.Visibility = Visibility.Visible;
                PrintCanvas.UpdateLayout();
            }

            _printPreviewPages.Add(page);

            return textLink;
        }

        public Canvas PrintCanvas
        {
            get
            {
                return _scenarioPage.FindName("PrintCanvas") as Canvas;
            }
        }

        public event EventHandler<bool> PrintDone;
    }
}
