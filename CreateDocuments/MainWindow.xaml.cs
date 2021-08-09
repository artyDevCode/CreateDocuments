using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MMLib.RapidPrototyping.Generators;

namespace CreateDocuments
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();
            fileType.SelectedIndex = 1;
        }




        public  async void WordDocCreator(string fileName, string randomText, int paragraphCount, int maxSentencesinParagraph, int fileCount, string fileType)
        {
            //Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            //dlg.FileName = fileName; // "GeneralOrder"; // Default file name
            //dlg.DefaultExt = ".docx"; // Default file extension
            //dlg.Filter = "Word Documents|*.doc;*.docx|Excel Worksheets|*.xls|PowerPoint Presentations|*.ppt" +
            // "|Office Files|*.doc;*.docx;*.xls;*.ppt" +
            // "|All Files|*.*";

            //// Show save file dialog box
            //Nullable<bool> result = dlg.ShowDialog();
            //if (result == true)
            var result = true;
            if ( fileName != string.Empty & randomText != string.Empty & paragraphCount > 0 & maxSentencesinParagraph > 0 & fileCount > 0)
            { 
            //Create a missing variable for missing value
            object missing = System.Reflection.Missing.Value;
            Microsoft.Office.Interop.Word.Application winword = new Microsoft.Office.Interop.Word.Application();
                if (result)
                {

                    ///Create the word document based on the res Obj
                    try
                    {
                        await Task.Run(() =>
                        {
                            for (var i = 1; i < fileCount + 1; i++)
                            {
                                Application.Current.Dispatcher.Invoke(() =>
                                {
                                    statusBox.Text = "Processing....";
                                    numberFileCreated.Text = (i).ToString();
                                });


                            //  //Set status for word application is to be visible or not.
                            winword.Visible = false;
                                winword.Options.SavePropertiesPrompt = false;

                            //  //Create a new document
                            Microsoft.Office.Interop.Word.Document document = winword.Documents.Add(ref missing, ref missing, ref missing, ref missing);


                            //Set the page style
                            document.Styles["Normal"].NoSpaceBetweenParagraphsOfSameStyle = false;
                                document.PageSetup.LeftMargin = 90;
                                document.PageSetup.RightMargin = 90;
                                document.PageSetup.HeaderDistance = 36;
                                document.PageSetup.FooterDistance = 36;
                                document.DefaultTabStop = 36;

                            //adding text to document
                            document.Content.SetRange(0, 0);
                                document.Styles["Normal"].Font.Name = "Times New Roman";
                            //Add paragraph with Normal style
                            Microsoft.Office.Interop.Word.Paragraph para1 = document.Content.Paragraphs.Add(ref missing);
                                object styleHeading1 = "Normal";
                                para1.Range.set_Style(ref styleHeading1);
                                para1.TabStops.ClearAll();
                                para1.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                                para1.Range.ParagraphFormat.LineSpacingRule = Microsoft.Office.Interop.Word.WdLineSpacing.wdLineSpaceSingle;
                                para1.Range.ParagraphFormat.SpaceAfter = 0;
                                winword.Selection.TypeParagraph();
                                winword.Selection.TypeParagraph();


                            //Adding first page title and text as per original General Order document.
                            para1.Range.Text = randomText;
                                para1.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                                para1.Range.Bold = -1;
                                para1.Range.ParagraphFormat.LeftIndent = 0;
                                para1.Range.ParagraphFormat.PageBreakBefore = 0;
                                para1.Range.ParagraphFormat.LineSpacingRule = Microsoft.Office.Interop.Word.WdLineSpacing.wdLineSpaceSingle;
                                para1.Range.ParagraphFormat.SpaceAfter = 0;
                                para1.Range.InsertParagraphAfter();
                                para1.Range.InsertParagraph();

                                para1.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;
                                para1.Range.Bold = 0;
                                para1.Range.Text = GenerateText(paragraphCount, maxSentencesinParagraph); // get_unique_string(10); // "=rand(10,10)\r\n";
                            para1.Range.Copy();
                                para1.Range.Paste();
                                para1.Range.InsertParagraphAfter();
                                para1.Range.InsertParagraph();

                            // Save document
                            if (fileType.ToLower() == "pdf")
                                    document.SaveAs(fileName + i, Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF);
                                else
                                    document.SaveAs(fileName + i, Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatDocument);

                                document.Saved = true;

                            //Release the Word COM object
                            ((Microsoft.Office.Interop.Word._Document)document).Close(ref missing, ref missing, ref missing);
                            }
                        });

                    }
                    catch (Exception e)
                    {
                        statusBox.Text = "Error...." + e.Message;
                        ((Microsoft.Office.Interop.Word._Application)winword).Quit(ref missing, ref missing, ref missing);

                    }
                    statusBox.Text = "Completed.";
                    ((Microsoft.Office.Interop.Word._Application)winword).Quit(ref missing, ref missing, ref missing);

                }
            }
            else
            {
                MessageBox.Show("There are some invalid fields");
            }
        }

        public static string GenerateText(int paragraphCount, int maxSentencesinParagraph)
        {
            WordGenerator generator = new WordGenerator();
            var randomWord = generator.Next();
            
            LoremIpsumGenerator loremIpsumGenerator = new LoremIpsumGenerator();
            return loremIpsumGenerator.Next(paragraphCount, maxSentencesinParagraph);


        }
        public static string get_unique_string(int string_length)
        {
            //using (var rng = new RNGCryptoServiceProvider())
            //{
            //    var bit_count = (string_length * 6);
            //    var byte_count = ((bit_count + 7) / 8); // rounded up
            //    var bytes = new byte[byte_count];
            //    rng.GetBytes(bytes);
            //    return Convert.ToBase64String(bytes);
            //}
            ////////////////////////////
            var random = new Random();
            string[] words = { "an", "automobile", "or", "motor", "car", "is", "a", "wheeled", "motor", "vehicle", "used", "for", "transporting", "passengers", "which", "also", "carries", "its", "own", "engine", "or" };

            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < 10; i++)
            {
                // Select a random word from the array
                builder.Append(words[random.Next(words.Length)]).Append(" ");
            }

            string sentence = builder.ToString().Trim() + ". ";

            // Set the first letter of the first word in the sentenece to uppercase
            sentence = char.ToUpper(sentence[0]) + sentence.Substring(1);

            builder = new StringBuilder();
            builder.Append(sentence);

            return builder.ToString();
            ///////////////


        }

        private void StartProcess_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            statusBox.Text = "Started....";
            WordDocCreator(fileName.Text, searchText.Text, Convert.ToInt16(paragraphCount.Text), Convert.ToInt16(maxSentencesinParagraph.Text), Convert.ToInt16(fileCount.Text), fileType.Text);
        }

        private void validate_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Back)
                e.Handled = false;
            else
            {
                Regex regex = new Regex("^[0-9]+$");
                e.Handled = !regex.IsMatch(e.Key.ToString().Remove(0, 1));
            }
        }

       
    }

}