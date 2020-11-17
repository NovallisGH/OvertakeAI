﻿using System;
using System.Collections.Generic;
using System.Linq;
using OvertakeAI;

namespace NeuralNetworkSolution
{
    class NNSolution
    {
        private static readonly List<string> PossibleResults = new List<string> { "FALSE", "TRUE" };

        public void Run()
        {
            Console.WriteLine("Neural Network");
            Console.Write("Amount of data to train: ");
            int amountOfData = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine();

            var trainDataSet = GetInputs(amountOfData).ToArray();

            var network = new NeuralNetwork(4, 5, 3, 0.2);

            Console.Write("Amount of epochs: ");
            int epochs = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine();

            Console.WriteLine($"Training network with {trainDataSet.Length} samples using {epochs} epochs...");

            for (var epoch = 0; epoch < epochs; epoch++)
                foreach (var data in trainDataSet)
                {
                    var targets = new[] { 0.01, 0.01, 0.01 };
                    targets[PossibleResults.IndexOf(data.Last())] = 0.99;

                    var dataList = data.Take(4).Select(double.Parse).ToArray();
                    network.Train(NormalizeData(dataList), targets);
                }

            var scoreCard = new List<bool>();

            Console.Write("Amount of data to predict: ");
            amountOfData = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine();

            var testDataSet = GetInputs(amountOfData).ToArray();

            foreach (var data in testDataSet)
            {
                var result = network.Query(NormalizeData(data.Take(4).Select(double.Parse).ToArray())).ToList();
                var answer = PossibleResults[PossibleResults.IndexOf(data.Last())];
                var predicted = PossibleResults[result.IndexOf(result.Max())];

                scoreCard.Add(answer == predicted);
            }

            Console.WriteLine($"Performance is {(scoreCard.Count(x => x) / Convert.ToDouble(scoreCard.Count)) * 100}%");
        }

        public static string[][] GetInputs(int train)
        {
            Library.Overtake overtake;
            string[][] data = new string[train][];

            for (int i = 0; i < train; i++)
            {
                overtake = OvertakeData.GetData();
                data[i] = new string[4] { overtake.InitialSeparationM.ToString(), overtake.OvertakingSpeedMPS.ToString(), overtake.OncomingSpeedMPS.ToString(), overtake.Success.ToString() };
            }

            return data;
        }

        private static double[] NormalizeData(double[] input)
        {
            var normalized = new[]
            {
                input[0],
                input[1],
                input[2],
                input[3]
            };

            return normalized;
        }
    }
}
