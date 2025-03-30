
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.IO.Image;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Kernel.Colors;
using iText.Layout.Borders;
using System.Globalization;

namespace service.Service
{
    public class PdfService
    {
        private readonly string _logoPath;
        private readonly string _fontPath;
        private readonly string _signaturePath;

        public PdfService(string logoPath = "Image\\SyncER (5).png",
                          string fontPath = "c:/windows/fonts/arial.ttf",
                          string signaturePath = "Image\\signature.png")
        {
            _logoPath = logoPath;
            _fontPath = fontPath;
            _signaturePath = signaturePath;
        }

    
        public byte[] GenerateTreatmentSummaryPdf(
            string patientName,
            string patientIdentity,
            DateTime birthDate,
            string diagnosis,
            string procedure,
            string recommendations,
            string doctorName = "Dr. Ruth Seadya",
            string hospitalDepartment = "Emergency Medicine Department",
            string hospitalName = "The Medical Center")
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var writer = new PdfWriter(memoryStream))
                {
                    using (var pdf = new PdfDocument(writer))
                    {
                        var document = new Document(pdf);

                        PdfFont font = PdfFontFactory.CreateFont(_fontPath, PdfEncodings.IDENTITY_H, PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);
                        PdfFont boldFont = PdfFontFactory.CreateFont(_fontPath, PdfEncodings.IDENTITY_H, PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);

                        document.SetFont(font);
                        document.SetTextAlignment(TextAlignment.LEFT);

                        AddHeader(document, hospitalName, hospitalDepartment, boldFont);

                        AddPatientDetails(document, patientName, patientIdentity, birthDate, boldFont);

                        Paragraph titleParagraph = new Paragraph("MEDICAL TREATMENT SUMMARY")
                            .SetFontSize(18)
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetMarginTop(15)
                            .SetMarginBottom(10)
                            .SetFont(boldFont);
                        document.Add(titleParagraph);

                        LineSeparator line = new LineSeparator(new SolidLine(1f));
                        line.SetMarginTop(5).SetMarginBottom(15);
                        document.Add(line);

                        AddTreatmentDetails(document, diagnosis, procedure, recommendations, boldFont);

                        string currentDate = DateTime.Now.ToString("MMMM d, yyyy", new CultureInfo("en-US"));
                        string currentTime = DateTime.Now.ToString("h:mm tt", new CultureInfo("en-US"));

                        AddDoctorSignature(document, doctorName, currentDate, currentTime, boldFont);

                        AddFooter(document, boldFont);

                        document.Close();
                    }
                }
                return memoryStream.ToArray();
            }
        }

        
        private void AddHeader(Document document, string hospitalName, string departmentName, PdfFont boldFont)
        {
            Table headerTable = new Table(2).UseAllAvailableWidth();

            Paragraph hospitalParagraph = new Paragraph(hospitalName)
                .SetFontSize(16)
                .SetFont(boldFont);

            Cell hospitalCell = new Cell()
                .Add(hospitalParagraph)
                .Add(new Paragraph(departmentName).SetFontSize(14))
                .SetBorder(Border.NO_BORDER)
                .SetTextAlignment(TextAlignment.LEFT);

            Cell logoCell = new Cell().SetBorder(Border.NO_BORDER);
            if (File.Exists(_logoPath))
            {
                Image logo = new Image(ImageDataFactory.Create(_logoPath))
                    .ScaleToFit(120, 60)
                    .SetHorizontalAlignment(HorizontalAlignment.RIGHT);
                logoCell.Add(logo);
            }
            else
            {
                logoCell.Add(new Paragraph("LOGO").SetTextAlignment(TextAlignment.RIGHT));
            }

            headerTable.AddCell(hospitalCell);
            headerTable.AddCell(logoCell);
            document.Add(headerTable);

            // Separator line after header
            LineSeparator headerLine = new LineSeparator(new SolidLine(1.5f));
            headerLine.SetMarginTop(5).SetMarginBottom(15);
            document.Add(headerLine);
        }

      
        private void AddPatientDetails(Document document, string patientName, string patientId, DateTime birthDate, PdfFont boldFont)
        {
            Table patientTable = new Table(UnitValue.CreatePercentArray(new float[] { 30, 70 }))
                .UseAllAvailableWidth()
                .SetWidth(UnitValue.CreatePercentValue(100))
                .SetMarginBottom(10);

            // Patient Name
            patientTable.AddCell(CreateCell("Patient Name:", true, boldFont));
            patientTable.AddCell(CreateCell(patientName, false, null));

            patientTable.AddCell(CreateCell("ID Number:", true, boldFont));
            patientTable.AddCell(CreateCell(patientId, false, null));

            string formattedBirthDate = birthDate.ToString("MMMM d, yyyy", new CultureInfo("en-US"));
            patientTable.AddCell(CreateCell("Date of Birth:", true, boldFont));
            patientTable.AddCell(CreateCell(formattedBirthDate, false, null));

            int age = CalculateAge(birthDate);
            patientTable.AddCell(CreateCell("Age:", true, boldFont));
            patientTable.AddCell(CreateCell(age.ToString(), false, null));

            document.Add(patientTable);
        }

       
        private void AddTreatmentDetails(Document document, string diagnosis, string procedure, string recommendations, PdfFont boldFont)
        {
            Table treatmentTable = new Table(1)
                .UseAllAvailableWidth()
                .SetWidth(UnitValue.CreatePercentValue(100));

            // Diagnosis
            treatmentTable.AddCell(CreateLabeledSection("DIAGNOSIS", diagnosis, boldFont));

            // Procedure
            treatmentTable.AddCell(CreateLabeledSection("PROCEDURE", procedure, boldFont));

            // Recommendations
            treatmentTable.AddCell(CreateLabeledSection("RECOMMENDATIONS", recommendations, boldFont));

            document.Add(treatmentTable);
        }

        private void AddDoctorSignature(Document document, string doctorName, string currentDate, string currentTime, PdfFont boldFont)
        {
            document.Add(new Paragraph(" ").SetMarginTop(20));

            Table signatureTable = new Table(2).UseAllAvailableWidth();

            Cell dateTimeCell = new Cell()
                .Add(new Paragraph("Date: " + currentDate))
                .Add(new Paragraph("Time: " + currentTime))
                .SetBorder(Border.NO_BORDER)
                .SetTextAlignment(TextAlignment.LEFT);

            // Doctor signature
            Cell signatureCell = new Cell().SetBorder(Border.NO_BORDER);
            if (File.Exists(_signaturePath))
            {
                Image signature = new Image(ImageDataFactory.Create(_signaturePath))
                    .ScaleToFit(100, 50)
                    .SetHorizontalAlignment(HorizontalAlignment.RIGHT);
                signatureCell.Add(signature);
            }

            signatureCell.Add(new Paragraph(doctorName).SetTextAlignment(TextAlignment.RIGHT).SetFont(boldFont));
            signatureCell.Add(new Paragraph("Attending Physician").SetTextAlignment(TextAlignment.RIGHT));

            signatureTable.AddCell(dateTimeCell);
            signatureTable.AddCell(signatureCell);
            document.Add(signatureTable);
        }

        private void AddFooter(Document document, PdfFont boldFont)
        {
            document.Add(new Paragraph(" ").SetMarginTop(30));

            Paragraph disclaimerParagraph = new Paragraph("* This document is a summary of medical treatment only and does not constitute binding medical advice.")
                .SetFontSize(8)
                .SetTextAlignment(TextAlignment.CENTER);
            document.Add(disclaimerParagraph);

            
            Paragraph emergencyParagraph = new Paragraph("EMERGENCY CONTACT: 1-800-123-456")
                .SetFontSize(10)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetMarginTop(5)
                .SetFont(boldFont);
            document.Add(emergencyParagraph);
        }

      
        private Cell CreateLabeledSection(string label, string content, PdfFont boldFont)
        {
            Cell cell = new Cell()
                .SetPadding(10)
                .SetBorderRadius(new BorderRadius(3))
                .SetBackgroundColor(new DeviceRgb(248, 249, 250))
                .SetBorder(new SolidBorder(ColorConstants.LIGHT_GRAY, 0.5f))
                .SetMarginBottom(10);

            Paragraph labelParagraph = new Paragraph(label)
                .SetFontSize(12)
                .SetMarginBottom(5)
                .SetFont(boldFont);
            cell.Add(labelParagraph);

            cell.Add(new Paragraph(content)
                .SetFontSize(11)
                .SetMarginBottom(5));

            return cell;
        }

        
        private Cell CreateCell(string text, bool isBold, PdfFont boldFont)
        {
            Paragraph paragraph = new Paragraph(text).SetFontSize(11);
            if (isBold && boldFont != null)
            {
                paragraph.SetFont(boldFont);
            }

            Cell cell = new Cell()
                .Add(paragraph)
                .SetBorder(Border.NO_BORDER)
                .SetPadding(5);

            return cell;
        }

        
        private int CalculateAge(DateTime birthDate)
        {
            var today = DateTime.Today;
            int age = today.Year - birthDate.Year;

            if (birthDate.Date > today.AddYears(-age))
            {
                age--;
            }

            return age;
        }
    }

}

























//עברית

//using System;
//using System.IO;
//using System.Linq;
//using iText.Kernel.Pdf;
//using iText.Layout;
//using iText.Layout.Element;
//using iText.Layout.Properties;
//using iText.IO.Font;
//using iText.Kernel.Font;
//using iText.IO.Image;
//using iText.Kernel.Pdf.Canvas.Draw;
//using iText.Kernel.Colors;
//using iText.Layout.Borders;
//using System.Globalization;

//namespace service.Service
//{
//    public class PdfService
//    {
//        private readonly string _logoPath;
//        private readonly string _fontPath;
//        private readonly string _signaturePath;

//        public PdfService(string logoPath = "Image\\SyncER (5).png",
//                          string fontPath = "c:/windows/fonts/arial.ttf",
//                          string signaturePath = "Image\\signature.png")
//        {
//            _logoPath = logoPath;
//            _fontPath = fontPath;
//            _signaturePath = signaturePath;
//        }
//        string FixRTL(string text) => new string(text.Reverse().ToArray());

//        /// <summary>
//        /// מייצר מסמך PDF של סיכום טיפול רפואי
//        /// </summary>
//        public byte[] GenerateTreatmentSummaryPdf(

//            string patientName,
//            string patientIdentity,
//            DateTime birthDate,
//            string diagnosis,
//            string procedure,
//            string recommendations,
//            string doctorName = "ד״ר רות סעדיה",
//            string hospitalDepartment = "המחלקה לרפואה דחופה",
//            string hospitalName = "המרכז הרפואי")
//        {


//            using (var memoryStream = new MemoryStream())
//            {
//                using (var writer = new PdfWriter(memoryStream))
//                {
//                    using (var pdf = new PdfDocument(writer))
//                    {
//                        var document = new Document(pdf);


//                        // טעינת גופן תומך בעברית
//                        PdfFont hebrewFont = PdfFontFactory.CreateFont(_fontPath, PdfEncodings.IDENTITY_H, PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);

//                        // הגדרת הכיוון של המסמך לימין לשמאל
//                        document.SetTextAlignment(TextAlignment.RIGHT);
//                        document.SetFont(hebrewFont);

//                        // הוספת כותרת ולוגו בחלק העליון
//                        AddHeader(document, FixRTL(hospitalName), FixRTL(hospitalDepartment));

//                        // פרטי מטופל
//                        AddPatientDetails(document, patientName, patientIdentity, birthDate);

//                        // כותרת המסמך - שימוש ב-setBold() על Paragraph
//                        Paragraph titleParagraph = new Paragraph(FixRTL("סיכום טיפול רפואי"))
//                            .SetFont(hebrewFont)
//                            .SetFontSize(18)
//                            .SetTextAlignment(TextAlignment.CENTER)
//                            .SetMarginTop(15)
//                            .SetMarginBottom(10);
//                       // titleParagraph.SetBold(); // קריאה נפרדת ל-SetBold
//                        document.Add(titleParagraph);

//                        // קו הפרדה
//                        LineSeparator line = new LineSeparator(new SolidLine(1f));
//                        line.SetMarginTop(5).SetMarginBottom(15);
//                        document.Add(line);

//                        // פרטי הטיפול
//                        AddTreatmentDetails(document, diagnosis, procedure, recommendations);

//                        // תאריך ושעה
//                        string currentDate = DateTime.Now.ToString("dd/MM/yyyy", new CultureInfo("he-IL"));
//                        string currentTime = DateTime.Now.ToString("HH:mm", new CultureInfo("he-IL"));

//                        // חתימה ופרטי רופא
//                        AddDoctorSignature(document, doctorName, currentDate, currentTime);

//                        // הוספת מספור עמודים והערת שוליים
//                        AddFooter(document);

//                        document.Close();
//                    }
//                }
//                return memoryStream.ToArray();
//            }
//        }

//        /// <summary>
//        /// הוספת כותרת ולוגו בחלק העליון של המסמך
//        /// </summary>
//        private void AddHeader(Document document, string hospitalName, string departmentName)
//        {
//            // יצירת טבלה לכותרת עליונה
//            Table headerTable = new Table(2).UseAllAvailableWidth();

//            // צד ימין - פרטי בית החולים
//            Paragraph hospitalParagraph = new Paragraph(hospitalName).SetFontSize(16);
//           // hospitalParagraph.SetBold(); // קריאה נפרדת ל-SetBold

//            Cell hospitalCell = new Cell()
//                .Add(hospitalParagraph)
//                .Add(new Paragraph(departmentName).SetFontSize(14))
//                .SetBorder(Border.NO_BORDER)
//                .SetTextAlignment(TextAlignment.RIGHT);

//            // צד שמאל - לוגו
//            Cell logoCell = new Cell().SetBorder(Border.NO_BORDER);
//            if (File.Exists(_logoPath))
//            {
//                Image logo = new Image(ImageDataFactory.Create(_logoPath))
//                    .ScaleToFit(120, 60)
//                    .SetHorizontalAlignment(HorizontalAlignment.LEFT);
//                logoCell.Add(logo);
//            }
//            else
//            {
//                logoCell.Add(new Paragraph("לוגו").SetTextAlignment(TextAlignment.LEFT));
//            }

//            headerTable.AddCell(logoCell);
//            headerTable.AddCell(hospitalCell);
//            document.Add(headerTable);

//            // קו הפרדה אחרי הכותרת
//            LineSeparator headerLine = new LineSeparator(new SolidLine(1.5f));
//            headerLine.SetMarginTop(5).SetMarginBottom(15);
//            document.Add(headerLine);
//        }

//        /// <summary>
//        /// הוספת פרטי המטופל
//        /// </summary>
//        private void AddPatientDetails(Document document, string patientName, string patientId, DateTime birthDate)
//        {
//            string FixRTL(string text) => new string(text.Reverse().ToArray());

//            Table patientTable = new Table(2).UseAllAvailableWidth();
//            patientTable.SetWidth(UnitValue.CreatePercentValue(100));
//            //table.AddCell(CreateCell(FixRTL("אבחון:"), hebrewFont, true));

//            // מספר ת.ז
//            patientTable.AddCell(CreateCell(patientId, false));
//            _ = patientTable.AddCell(CreateCell(FixRTL("ת.ז:"), true));

//            // שם המטופל
//            patientTable.AddCell(CreateCell(FixRTL(patientName), false));
//            patientTable.AddCell(CreateCell(FixRTL("שם מטופל:"), true));

//            // תאריך לידה
//            string formattedBirthDate = birthDate.ToString("dd/MM/yyyy", new CultureInfo("he-IL"));
//            patientTable.AddCell(CreateCell((formattedBirthDate), false));
//            patientTable.AddCell(CreateCell(FixRTL("תאריך לידה:"), true));


//            // גיל
//            int age = CalculateAge(birthDate);
//            patientTable.AddCell(CreateCell(age.ToString(), false));
//            patientTable.AddCell(CreateCell(FixRTL("גיל:"), true));

//            document.Add(patientTable);
//        }

//        /// <summary>
//        /// הוספת פרטי הטיפול
//        /// </summary>
//        private void AddTreatmentDetails(Document document, string diagnosis, string procedure, string recommendations)
//        {
//            string FixRTL(string text) => new string(text.Reverse().ToArray());

//            Table treatmentTable = new Table(1).UseAllAvailableWidth();
//            treatmentTable.SetWidth(UnitValue.CreatePercentValue(100));

//            // אבחנה
//            treatmentTable.AddCell(CreateLabeledSection(FixRTL("אבחנה:"), FixRTL(diagnosis)));

//            // הטיפול שבוצע
//            treatmentTable.AddCell(CreateLabeledSection(FixRTL("הטיפול שבוצע:"), FixRTL(procedure)));

//            // תרופות
//            //if (!string.IsNullOrEmpty(medications))
//            //{
//            //    treatmentTable.AddCell(CreateLabeledSection("תרופות:", medications));
//            //}

//            // המלצות להמשך טיפול
//            treatmentTable.AddCell(CreateLabeledSection(FixRTL("המלצות להמשך:"), FixRTL(recommendations)));

//            document.Add(treatmentTable);
//        }

//        /// <summary>
//        /// הוספת חתימת הרופא ופרטי החתימה
//        /// </summary>
//        private void AddDoctorSignature(Document document, string doctorName, string currentDate, string currentTime)
//        {
//            string FixRTL(string text) => new string(text.Reverse().ToArray());

//            // הוספת רווח
//            document.Add(new Paragraph(" ").SetMarginTop(20));

//            Table signatureTable = new Table(2).UseAllAvailableWidth();

//            // פרטי תאריך ושעה
//            Cell dateTimeCell = new Cell()
//                .Add(new Paragraph((currentDate)+FixRTL("תאריך: ") ) )
//                .Add(new Paragraph((currentTime)+ FixRTL("שעה: ")))
//                .SetBorder(Border.NO_BORDER)
//                .SetTextAlignment(TextAlignment.RIGHT);

//            // חתימת רופא
//            Cell signatureCell = new Cell().SetBorder(Border.NO_BORDER);
//            if (File.Exists(_signaturePath))
//            {
//                Image signature = new Image(ImageDataFactory.Create(_signaturePath))
//                    .ScaleToFit(100, 50)
//                    .SetHorizontalAlignment(HorizontalAlignment.LEFT);
//                signatureCell.Add(signature);
//            }

//            signatureCell.Add(new Paragraph(FixRTL(doctorName)).SetTextAlignment(TextAlignment.LEFT));
//            signatureCell.Add(new Paragraph(FixRTL("רופא מטפל")).SetTextAlignment(TextAlignment.LEFT));

//            signatureTable.AddCell(signatureCell);
//            signatureTable.AddCell(dateTimeCell);
//            document.Add(signatureTable);
//        }

//        /// <summary>
//        /// הוספת הערת שוליים
//        /// </summary>
//        private void AddFooter(Document document)
//        {
//            string FixRTL(string text) => new string(text.Reverse().ToArray());

//            // הערת שוליים
//            document.Add(new Paragraph(" ").SetMarginTop(30));

//            // תיקון לבעיה עם SetItalic
//            Paragraph disclaimerParagraph = new Paragraph(FixRTL("* מסמך זה הוא סיכום טיפול רפואי בלבד ואינו מהווה חוות דעת רפואית מחייבת."))
//                .SetFontSize(8)
//                .SetTextAlignment(TextAlignment.CENTER);
//           // disclaimerParagraph.SetItalic(); // קריאה נפרדת ל-SetItalic
//            document.Add(disclaimerParagraph);

//            // תיקון לבעיה עם SetBold
//            Paragraph emergencyParagraph = new Paragraph(FixRTL("טלפון למקרי חירום: 1800-123-456"))
//                .SetFontSize(10)
//                .SetTextAlignment(TextAlignment.CENTER)
//                .SetMarginTop(5);
//            //emergencyParagraph.SetBold(); // קריאה נפרדת ל-SetBold
//            document.Add(emergencyParagraph);
//        }

//        /// <summary>
//        /// יוצר תא עם תווית וטקסט בפסקה אחת
//        /// </summary>
//        private Cell CreateLabeledSection(string label, string content)
//        {
//            Cell cell = new Cell()
//                .SetPadding(5)
//                .SetBorder(new SolidBorder(ColorConstants.LIGHT_GRAY, 0.5f));

//            Paragraph labelParagraph = new Paragraph(label)
//                .SetFontSize(12)
//                .SetMarginBottom(3);
//           // labelParagraph.SetBold(); // קריאה נפרדת ל-SetBold
//            cell.Add(labelParagraph);

//            cell.Add(new Paragraph(content)
//                .SetFontSize(11)
//                .SetMarginBottom(5));

//            return cell;
//        }

//        /// <summary>
//        /// יוצר תא בסיסי לטבלה
//        /// </summary>
//        private Cell CreateCell(string text, bool isBold)
//        {
//            Paragraph paragraph = new Paragraph(text).SetFontSize(11);
//            if (isBold)
//            {
//                //paragraph.SetBold(); // קריאה נפרדת ל-SetBold
//            }

//            Cell cell = new Cell()
//                .Add(paragraph)
//                .SetBorder(Border.NO_BORDER)
//                .SetPadding(5);

//            return cell;
//        }

//        /// <summary>
//        /// מחשב את גיל המטופל
//        /// </summary>
//        private int CalculateAge(DateTime birthDate)
//        {
//            var today = DateTime.Today;
//            int age =( today.Year - birthDate.Year)/100;

//            if (birthDate.Date > today.AddYears(-age))
//            {
//                age--;
//            }

//            return age;
//        }
//    }

//}