using System;
using System.Collections.Generic;
using CustomDataSource.Model;

namespace CustomDataSource
{
    public static class SamplePaymentEntries
    {
        public static List<PaymentReportEntry> SampleData()
        {
            var r = new Random(DateTime.Now.Millisecond);
            string[] names =
            {
                GetName(r), GetName(r), GetName(r), GetName(r), GetName(r), GetName(r), GetName(r), GetName(r),
                GetName(r), GetName(r)
            };
            string[] addresses =
            {
                GetAddress(r), GetAddress(r), GetAddress(r), GetAddress(r), GetAddress(r), GetAddress(r),
                GetAddress(r), GetAddress(r), GetAddress(r), GetAddress(r)
            };
            var data = new List<PaymentReportEntry>();
            for (var i = 0; i < names.Length; i++)
            {
                var name = names[i];
                var address = addresses[i];
                var numEntries = r.Next(2, 6);
                for (var j = 0; j < numEntries; j++)
                {
                    var transactionDate = RandomDay(r);
                    var payee = string.Format($"Payee: {j}");
                    var refStr = (j == 0)
                        ? "This is a very long reference line which will go over two lines"
                        : "Reference";
                    var reference = string.Format($"{refStr}: {j}");
                    var amount = RandomNonNegativeDecimal(r, 5, 2);
                    data.Add(new PaymentReportEntry(i + 1, name, address, "Phone Number", "Email", transactionDate,
                        payee, reference, amount));
                }
            }
            data.Sort();
            return data;
        }

        private static string GetName(Random r)
        {
            string[] firstNames = { "David", "Charles", "Una", "Helen", "Anne", "Janet" };
            string[] lastNames = { "McCallum", "Walsh", "Wilson", "Frew", "Brown", "Butchart" };
            return string.Format($"{firstNames[r.Next(firstNames.Length)]} {lastNames[r.Next(lastNames.Length)]}");
        }

        private static DateTime RandomDay(Random r)
        {
            var start = new DateTime(2016, 1, 1);
            var range = (DateTime.Today - start).Days;
            return start.AddDays(r.Next(range));
        }

        private static decimal RandomNonNegativeDecimal(Random randomNumberGenerator, int precision, int scale)
        {
            if (randomNumberGenerator == null)
                throw new ArgumentNullException(nameof(randomNumberGenerator));
            if (!(precision >= 1 && precision <= 28))
                throw new ArgumentOutOfRangeException(nameof(precision), precision, @"Precision must be between 1 and 28.");
            if (!(scale >= 0 && scale <= precision))
                throw new ArgumentOutOfRangeException(nameof(scale), precision, @"Scale must be between 0 and precision.");

            var d = 0m;
            for (var i = 0; i < precision; i++)
            {
                var r = randomNumberGenerator.Next(0, 10);
                d = d * 10m + r;
            }
            for (var s = 0; s < scale; s++)
            {
                d /= 10m;
            }
            return d;
        }

        private static string GetAddress(Random r)
        {
            int[] houseNumbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            string[] streetNames =
                {"Bingham Broadway", "Pleasance", "Telford Road", "Muirhouse Drive", "Kinloch Drive", "Birrens Road"};
            string[] cities = { "Edinburgh", "Motherwell", "London", "Belfast" };
            string[] postCodes = { "EH15 3JL", "EH8 9TN", "ML1 2AB", "BT9 3HG" };
            return string.Format($"{houseNumbers[r.Next(houseNumbers.Length)]} {streetNames[r.Next(streetNames.Length)]}{Environment.NewLine}{cities[r.Next(cities.Length)]}{Environment.NewLine}{postCodes[r.Next(postCodes.Length)]}");
        }


    }
}
