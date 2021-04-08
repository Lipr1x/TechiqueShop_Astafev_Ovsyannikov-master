using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Text;

namespace TechiqueShopBusinessLogic.FileModels
{
    static class SaveToWord
    {
        /// <summary>
        /// Создание документа
        /// </summary>
        /// <param name="info"></param>
        public static void CreateDocument(WordExcelInfo info)
        {
            using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(info.FileName, WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                mainPart.Document = new Document();
                Body docBody = mainPart.Document.AppendChild(new Body());
                docBody.AppendChild(CreateParagraph(new WordParagraph
                {
                    Texts = new List<(string, WordTextProperties)> { (info.Title, new WordTextProperties { Bold = true, Size = "24", }) },
                    TextProperties = new WordTextProperties
                    {
                        Size = "24",
                        JustificationValues = (DocumentFormat.OpenXml.Math.JustificationValues)JustificationValues.Center
                    }
                }));
                if (info.ComponentAssembly != null)
                {
                    string components = "";
                    for (int i = 0; i < info.ChoosedComponents.Count; i++)
                    {
                        components += info.ChoosedComponents[i];
                        if (i != info.ChoosedComponents.Count - 1)
                        {
                            components += ", ";
                        }
                    }
                    docBody.AppendChild(CreateParagraph(new WordParagraph
                    {
                        Texts = new List<(string, WordTextProperties)> {
                                ("Выбранные комплектующие: ", new WordTextProperties {Bold = true, Size = "24", })},
                        TextProperties = new WordTextProperties
                        {
                            Size = "24",
                            JustificationValues = (DocumentFormat.OpenXml.Math.JustificationValues)JustificationValues.Both
                        }
                    })); ;
                    docBody.AppendChild(CreateParagraph(new WordParagraph
                    {
                        Texts = new List<(string, WordTextProperties)> {
                            (components, new WordTextProperties {Bold = false, Size = "24", })},
                        TextProperties = new WordTextProperties
                        {
                            Size = "24",
                            JustificationValues = (DocumentFormat.OpenXml.Math.JustificationValues)JustificationValues.Both
                        }
                    })); ;
                    foreach (var compAssem in info.ComponentAssembly)
                    {
                        docBody.AppendChild(CreateParagraph(new WordParagraph
                        {
                            Texts = new List<(string, WordTextProperties)> {
                                (compAssem.AssemblyName, new WordTextProperties {Bold = true, Size = "24", }) ,
                                (" : \n", new WordTextProperties {Bold = false, Size = "24", })},
                            TextProperties = new WordTextProperties
                            {
                                Size = "24",
                                JustificationValues = (DocumentFormat.OpenXml.Math.JustificationValues)JustificationValues.Both
                            }
                        })); ;
                        foreach (string medication in compAssem.Components)
                        {
                            docBody.AppendChild(CreateParagraph(new WordParagraph
                            {
                                Texts = new List<(string, WordTextProperties)> {
                                ("\t" + medication, new WordTextProperties {Bold = false, Size = "24", })},
                                TextProperties = new WordTextProperties
                                {
                                    Size = "24",
                                    JustificationValues = (DocumentFormat.OpenXml.Math.JustificationValues)JustificationValues.Both
                                }
                            })); ;
                        }
                    }
                    docBody.AppendChild(CreateSectionProperties());
                }
                wordDocument.MainDocumentPart.Document.Save();
            }
        }
        /// <summary>
        /// Настройки страницы
        /// </summary>
        /// <returns></returns>
        private static SectionProperties CreateSectionProperties()
        {
            SectionProperties properties = new SectionProperties();
            PageSize pageSize = new PageSize
            {
                Orient = PageOrientationValues.Portrait
            };
            properties.AppendChild(pageSize);
            return properties;
        }
        /// <summary>
        /// Создание абзаца с текстом
        /// </summary>
        /// <param name="paragraph"></param>
        /// <returns></returns>
        private static Paragraph CreateParagraph(WordParagraph paragraph)
        {
            if (paragraph != null)
            {
                Paragraph docParagraph = new Paragraph();

                docParagraph.AppendChild(CreateParagraphProperties(paragraph.TextProperties));
                foreach (var run in paragraph.Texts)
                {
                    Run docRun = new Run();
                    RunProperties properties = new RunProperties();
                    properties.AppendChild(new FontSize { Val = run.Item2.Size });
                    if (run.Item2.Bold)
                    {
                        properties.AppendChild(new Bold());
                    }
                    docRun.AppendChild(properties);
                    docRun.AppendChild(new Text
                    {
                        Text = run.Item1,
                        Space =
                   SpaceProcessingModeValues.Preserve
                    });
                    docParagraph.AppendChild(docRun);
                }
                return docParagraph;
            }
            return null;
        }
        /// <summary>
        /// Задание форматирования для абзаца
        /// </summary>
        /// <param name="paragraphProperties"></param>
        /// <returns></returns>
        private static ParagraphProperties CreateParagraphProperties(WordTextProperties paragraphProperties)
        {
            if (paragraphProperties != null)
            {
                ParagraphProperties properties = new ParagraphProperties();
                properties.AppendChild(new Justification()
                {
                    //Val = paragraphProperties.JustificationValues
                });
                properties.AppendChild(new SpacingBetweenLines
                {
                    LineRule = LineSpacingRuleValues.Auto
                });
                properties.AppendChild(new Indentation());
                ParagraphMarkRunProperties paragraphMarkRunProperties = new
               ParagraphMarkRunProperties();
                if (!string.IsNullOrEmpty(paragraphProperties.Size))
                {
                    paragraphMarkRunProperties.AppendChild(new FontSize
                    {
                        Val = paragraphProperties.Size
                    });
                }
                properties.AppendChild(paragraphMarkRunProperties);
                return properties;
            }
            return null;
        }
    }
}
