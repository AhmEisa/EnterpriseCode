using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.IO;
using System.Text;
using BitMiracle.Docotic.Pdf;
using Tesseract;
using System;

namespace Enterprise.Try
{
    public class ExtractTextFromFiles
    {
        public static string ExtractText(string filePath)
        {
            if (!File.Exists(filePath)) return string.Empty;

            StringBuilder documentText = new StringBuilder();

            PdfReader pdfReader = new PdfReader(filePath);

            using (var engine = new TesseractEngine(@"C:\tessdata", "eng", EngineMode.Default))
            {
                for (int page = 1; page <= pdfReader.NumberOfPages; page++)
                {
                    if (documentText.Length > 0)
                        documentText.Append("\r\n\r\n");

                    //ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                    string currentText = PdfTextExtractor.GetTextFromPage(pdfReader, page);

                    if (!string.IsNullOrEmpty(currentText.Trim()))
                    {
                        //  currentText = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(currentText)));
                        documentText.Append(currentText);
                        continue;
                    }

                    // This page is not searchable.
                    // Save the page as a high-resolution image
                    PdfDrawOptions options = PdfDrawOptions.Create();
                    options.BackgroundColor = new PdfRgbColor(255, 255, 255);
                    options.HorizontalResolution = 300;
                    options.VerticalResolution = 300;

                    string pageImage = $"page_{page}.png";

                    //var currentPage = pdfReader.GetPageN(page);
                    //page.Save(pageImage, options);
                    // pdfReader.GetPageN(page).
                    // Perform OCR
                    using (Pix img = Pix.LoadFromFile(pageImage))
                    {
                        using (Page recognizedPage = engine.Process(img))
                        {
                            Console.WriteLine($"Mean confidence for page #{page}: {recognizedPage.GetMeanConfidence()}");

                            string recognizedText = recognizedPage.GetText();
                            documentText.Append(recognizedText);
                        }
                    }

                    File.Delete(pageImage);
                }
            }

            pdfReader.Close();
            return documentText.ToString();
        }

        public static string ExtractText2(string filePath)
        {

            using (var pdf = new BitMiracle.Docotic.Pdf.PdfDocument(filePath))
            {
                return pdf.GetText();
            }
        }

        public static string ExtractText3(string filePath)
        {
            var documentText = new StringBuilder();
            using (var pdf = new BitMiracle.Docotic.Pdf.PdfDocument(filePath))
            {
                using (var engine = new TesseractEngine(@"C:\tessdata", "eng", EngineMode.TesseractAndLstm))
                {
                    for (int i = 0; i < pdf.PageCount; ++i)
                    {
                        if (documentText.Length > 0)
                            documentText.Append("\r\n\r\n");

                        BitMiracle.Docotic.Pdf.PdfPage page = pdf.Pages[i];
                        string searchableText = page.GetText();

                        // Simple check if the page contains searchable text.
                        // We do not need to perform OCR in that case.
                        if (!string.IsNullOrEmpty(searchableText.Trim()))
                        {
                            documentText.Append(searchableText);
                            continue;
                        }

                        // This page is not searchable.
                        // Save the page as a high-resolution image
                        PdfDrawOptions options = PdfDrawOptions.Create();
                        options.BackgroundColor = new PdfRgbColor(255, 255, 255);
                        options.HorizontalResolution = 300;
                        options.VerticalResolution = 300;

                        string pageImage = $"page_{i}.png";
                        page.Save(pageImage, options);

                        // Perform OCR
                        using (Pix img = Pix.LoadFromFile(pageImage))
                        {
                            using (Page recognizedPage = engine.Process(img))
                            {
                                Console.WriteLine($"Mean confidence for page #{i}: {recognizedPage.GetMeanConfidence()}");

                                string recognizedText = recognizedPage.GetText();
                                documentText.Append(recognizedText);
                            }
                        }

                        File.Delete(pageImage);
                    }
                }
            }
            return documentText.ToString();
        }

        public static string ExtractText4(string filePath)
        {
            //String ocrSource = @"D:\Alice\DLL\Source\";
            //OCRHandler.SetTrainResourcePath(ocrSource);
            //PDFDocument pdf = new PDFDocument(@"C:\input.pdf");
            //BasePage page = pdf.GetPage(0);
            //Bitmap bmp = page.ConvertToImage();
            //OCRPage ocrPage = OCRHandler.Import(bmp);
            //ocrPage.Recognize();
            //ocrPage.SaveTo(MIMEType.TXT, @"C:\output.txt");
            return string.Empty;
        }

        //public static byte[] PerformOCRTesseract(byte[] image)
        //{
        //    // Specify that Tesseract use three 3 languages: English, Russian and Vietnamese.
        //    //string tesseractLanguages = "rus+eng+vie";
        //    string tesseractLanguages = "eng";

        //    // A path to a folder which contains languages data files and font file "pdf.ttf".
        //    // Language data files can be found here:
        //    // Good and fast: https://github.com/tesseract-ocr/tessdata_fast
        //    // or
        //    // Best and slow: https://github.com/tesseract-ocr/tessdata_best
        //    // Also this folder must have write permissions.
        //    string tesseractData = System.IO.Path.GetFullPath(@"..\..\tessdata\");

        //    // A path for a temporary PDF file (because Tesseract returns OCR result as PDF document)
        //    string tempFile = System.IO.Path.Combine(tesseractData, System.IO.Path.GetRandomFileName());

        //    try
        //    {
        //        using (Tesseract.IResultRenderer renderer = Tesseract.PdfResultRenderer.CreatePdfRenderer(tempFile, tesseractData, true))
        //        {
        //            using (renderer.BeginDocument("Serachablepdf"))
        //            {
        //                using (Tesseract.TesseractEngine engine = new Tesseract.TesseractEngine(tesseractData, tesseractLanguages, Tesseract.EngineMode.Default))
        //                {
        //                    engine.DefaultPageSegMode = Tesseract.PageSegMode.Auto;
        //                    using (MemoryStream msImg = new MemoryStream(image))
        //                    {
        //                        System.Drawing.Image imgWithText = System.Drawing.Image.FromStream(msImg);
        //                        for (int i = 0; i < imgWithText.GetFrameCount(System.Drawing.Imaging.FrameDimension.Page); i++)
        //                        {
        //                            imgWithText.SelectActiveFrame(System.Drawing.Imaging.FrameDimension.Page, i);
        //                            using (MemoryStream ms = new MemoryStream())
        //                            {
        //                                imgWithText.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
        //                                byte[] imgBytes = ms.ToArray();
        //                                using (Tesseract.Pix img = Tesseract.Pix.LoadFromMemory(imgBytes))
        //                                {
        //                                    using (var page = engine.Process(img, "Serachablepdf"))
        //                                    {
        //                                        renderer.AddPage(page);
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        return File.ReadAllBytes(tempFile + ".pdf");
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine();
        //        Console.WriteLine("Please be sure that you have Language data files (*.traineddata) in your folder \"tessdata\"");
        //        Console.WriteLine("The Language data files can be download from here: https://github.com/tesseract-ocr/tessdata_fast");
        //        Console.ReadKey();
        //        throw new Exception("Error Tesseract: " + e.Message);
        //    }
        //    finally
        //    {
        //        if (File.Exists(tempFile + ".pdf"))
        //            File.Delete(tempFile + ".pdf");
        //    }
        //}

    }
}
