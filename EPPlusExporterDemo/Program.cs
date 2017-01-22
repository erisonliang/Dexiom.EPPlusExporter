﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using Dexiom.EPPlusExporter;
using OfficeOpenXml;

namespace EPPlusExporterDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateSimpleSpreadsheet();
        }

        private static void CreateSimpleSpreadsheet()
        {
            Console.WriteLine("Create fake data...");
            var faker = new Faker<Employee>().CustomInstantiator(n => new Employee(new Person()));
            var data = faker.Generate(1000);

            Console.WriteLine("Export to Excel...");
            var exporter = new EnumerableExporter<Employee>(data)
                .Ignore(n => n.Phone)
                //.Ignore(n => n.DateOfBirth)
                .DisplayFormatFor(n => n.UserName, "==> {0}");
            
            var excelPackage = exporter.CreateExcelPackage();
            SaveAndOpenDocument(excelPackage);
        }

        public static void SaveAndOpenDocument(ExcelPackage excelPackage)
        {
            Console.WriteLine("Opening document");
            
            Directory.CreateDirectory("temp");
            var fileInfo = new FileInfo($"temp\\Test_{Guid.NewGuid():N}.xlsx");
            excelPackage.SaveAs(fileInfo);
            Process.Start(fileInfo.FullName);
        }
    }
}
