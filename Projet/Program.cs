using System;
using System.IO;
using Projet.DataStruc;
using Microsoft.ML;


namespace Projet
{
    public class Program
    {

        private static string TrainDataRelativePath = $"C:/Users/kayva/source/repos/Projet/Projet/Data/TestML.csv";
        private static string TestDataRelativePath = $"C:/Users/kayva/source/repos/Projet/Projet/Data//Train.csv";

        private static string TrainDataPath = GetAbsolutePath(TrainDataRelativePath);
        private static string TestDataPath = GetAbsolutePath(TestDataRelativePath);


        private static string ModelRelativePath = $"C:/Users/kayva/source/repos/Projet/Projet/Data//BalanceClassifier.zip";

        private static string ModelPath = GetAbsolutePath(ModelRelativePath);

        public static void Main(string[] args)
        {
            var mlContext = new MLContext();
            BuildTrainEvaluateAndSaveModel(mlContext);

            TestPrediction(mlContext);

            Console.WriteLine("=============== End of process, hit any key to finish ===============");
            Console.ReadKey();
        }


        private static void BuildTrainEvaluateAndSaveModel(MLContext mlContext)
        {
            // STEP 1: Common data loading configuration
            var trainingDataView = mlContext.Data.LoadFromTextFile<Struc>(TrainDataPath, hasHeader: true, separatorChar: ';');
            var testDataView = mlContext.Data.LoadFromTextFile<Struc>(TestDataPath, hasHeader: true, separatorChar: ';');

            // STEP 2: Concatenate the features and set the training algorithm
            var pipeline = mlContext.Transforms.Concatenate("Features", "a", "b", "phi", "X0", "Y0", "X0_in", "Y0_in", "short_axis", "long_axis")
                .Append(mlContext.BinaryClassification.Trainers.FastTree(labelColumnName: "Label", featureColumnName: "Features"));

            Console.WriteLine("=============== Training the model ===============");
            ITransformer trainedModel = pipeline.Fit(trainingDataView);
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("=============== Finish the train model. Push Enter ===============");
            Console.WriteLine("");
            Console.WriteLine("");

            Console.WriteLine("===== Evaluating Model's accuracy with Test data =====");
            var predictions = trainedModel.Transform(testDataView);

            var metrics = mlContext.BinaryClassification.Evaluate(data: predictions, labelColumnName: "Label", scoreColumnName: "Score");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine($"************************************************************");
            Console.WriteLine($"*       Metrics for {trainedModel.ToString()} binary classification model      ");
            Console.WriteLine($"*-----------------------------------------------------------");
            Console.WriteLine($"*       Accuracy: {metrics.Accuracy:P2}");
            Console.WriteLine($"*       Area Under Roc Curve:      {metrics.AreaUnderRocCurve:P2}");
            Console.WriteLine($"*       Area Under PrecisionRecall Curve:  {metrics.AreaUnderPrecisionRecallCurve:P2}");
            Console.WriteLine($"*       F1Score:  {metrics.F1Score:P2}");
            Console.WriteLine($"*       LogLoss:  {metrics.LogLoss:#.##}");
            Console.WriteLine($"*       LogLossReduction:  {metrics.LogLossReduction:#.##}");
            Console.WriteLine($"*       PositivePrecision:  {metrics.PositivePrecision:#.##}");
            Console.WriteLine($"*       PositiveRecall:  {metrics.PositiveRecall:#.##}");
            Console.WriteLine($"*       NegativePrecision:  {metrics.NegativePrecision:#.##}");
            Console.WriteLine($"*       NegativeRecall:  {metrics.NegativeRecall:P2}");
            Console.WriteLine($"************************************************************");
            Console.WriteLine("");
            Console.WriteLine("");

            Console.WriteLine("=============== Saving the model to a file ===============");
            mlContext.Model.Save(trainedModel, trainingDataView.Schema, ModelPath);
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("=============== Model Saved ============= ");
        }


        private static void TestPrediction(MLContext mlContext)
        {
            ITransformer trainedModel = mlContext.Model.Load(ModelPath, out var modelInputSchema);

            // Create prediction engine related to the loaded trained model
            var predictionEngine = mlContext.Model.CreatePredictionEngine<Struc, BalancePrediction>(trainedModel);

            foreach (var Balance in BalanceSample.balanceDataList)
            {
                var prediction = predictionEngine.Predict(Balance);

                Console.WriteLine($"=============== Single Prediction  ===============");
                Console.WriteLine($"a: {Balance.a} ");
                Console.WriteLine($"b: {Balance.b} ");
                Console.WriteLine($"phi: {Balance.phi} ");
                Console.WriteLine($"X0: {Balance.X0} ");
                Console.WriteLine($"Y0: {Balance.Y0} ");
                Console.WriteLine($"X0_in: {Balance.X0_in} ");
                Console.WriteLine($"Y0_in: {Balance.Y0_in} ");
                Console.WriteLine($"short_axis: {Balance.short_axis} ");
                Console.WriteLine($"long_axis: {Balance.long_axis} ");
                Console.WriteLine($"Prediction Value: {prediction.Prediction} ");
                Console.WriteLine($"Prediction: {(prediction.Prediction ? "Equilibre" : "perte d'equilibre")} ");
                Console.WriteLine($"Probability: {prediction.Probability} ");
                Console.WriteLine($"==================================================");
                Console.WriteLine("");
                Console.WriteLine("");
            }

        }


        public static string GetAbsolutePath(string relativePath)
        {
            FileInfo _dataRoot = new FileInfo(typeof(Program).Assembly.Location);
            string assemblyFolderPath = _dataRoot.Directory.FullName;

            string fullPath = Path.Combine(assemblyFolderPath, relativePath);

            return fullPath;

        }
    }
}
