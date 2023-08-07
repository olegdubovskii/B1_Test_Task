using Generator.Core;

Console.WriteLine(GC.GetTotalAllocatedBytes());
FileGenerator fileGenerator = new FileGenerator();
for (int i = 0; i < 100; i++ )
{
    fileGenerator.GenerateFiles($"test{i}.txt");
}

Console.WriteLine(GC.GetTotalAllocatedBytes());
Console.WriteLine(GC.CollectionCount(0));