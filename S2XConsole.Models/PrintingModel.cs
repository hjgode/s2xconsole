using S2XConsole.Views;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Packaging;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;
namespace S2XConsole.Models
{
	public class PrintingModel
	{
		public void Print(List<System.Drawing.Image> pages)
		{
			FixedDocument fixedDocument = this.CreateFixedDocument(pages);
			PrintDialog printDialog = new PrintDialog();
			if (printDialog.ShowDialog() == true)
			{
				printDialog.PrintDocument(fixedDocument.DocumentPaginator, string.Empty);
			}
		}
		public void PrintPreview(List<System.Drawing.Image> pages)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (Package package = Package.Open(memoryStream, FileMode.OpenOrCreate, FileAccess.ReadWrite))
				{
					string text = "memorystream://tempdata.xps";
					Uri uri = new Uri(text);
					PackageStore.RemovePackage(uri);
					PackageStore.AddPackage(uri, package);
					XpsDocument xpsDocument = new XpsDocument(package, CompressionOption.Normal, text);
					XpsDocumentWriter xpsDocumentWriter = XpsDocument.CreateXpsDocumentWriter(xpsDocument);
					FixedDocument fixedDocument = this.CreateFixedDocument(pages);
					xpsDocumentWriter.Write(fixedDocument.DocumentPaginator);
					xpsDocument.GetFixedDocumentSequence();
					xpsDocument.Close();
					PrintPreview printPreview = new PrintPreview(fixedDocument);
					printPreview.ShowDialog();
					PackageStore.RemovePackage(uri);
				}
			}
		}
		private FixedDocument CreateFixedDocument(List<System.Drawing.Image> pages)
		{
			FixedDocument fixedDocument = new FixedDocument();
			foreach (System.Drawing.Image current in pages)
			{
				FixedPage fixedPage = new FixedPage();
				MemoryStream memoryStream = new MemoryStream();
				current.Save(memoryStream, ImageFormat.Bmp);
				BitmapImage bitmapImage = new BitmapImage();
				bitmapImage.BeginInit();
				bitmapImage.StreamSource = memoryStream;
				bitmapImage.EndInit();
				System.Windows.Controls.Image image = new System.Windows.Controls.Image();
				image.Source = bitmapImage;
				fixedPage.Children.Add(image);
				PageContent pageContent = new PageContent();
				((IAddChild)pageContent).AddChild(fixedPage);
				fixedDocument.Pages.Add(pageContent);
			}
			return fixedDocument;
		}
	}
}
