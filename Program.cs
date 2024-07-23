using System;
using System.Threading;

class Program
{
    static Semaphore tobacco = new Semaphore(0, 1);
    static Semaphore paper = new Semaphore(0, 1);
    static Semaphore matches = new Semaphore(0, 1);
    static Semaphore agent = new Semaphore(1, 1);

    static void Main(string[] args)
    {
        Thread smokerWithTobacco = new Thread(SmokerWithTobacco);
        Thread smokerWithPaper = new Thread(SmokerWithPaper);
        Thread smokerWithMatches = new Thread(SmokerWithMatches);
        Thread agentThread = new Thread(Agent);

        smokerWithTobacco.Start();
        smokerWithPaper.Start();
        smokerWithMatches.Start();
        agentThread.Start();

        smokerWithTobacco.Join();
        smokerWithPaper.Join();
        smokerWithMatches.Join();
        agentThread.Join();
    }

    static void Agent()
    {
        Random rand = new Random();
        while (true)
        {
            agent.WaitOne();
            int choice = rand.Next(3);
            switch (choice)
            {
                case 0:
                    Console.WriteLine("Посредник кладет на стол бумагу и спички.");
                    paper.Release();
                    matches.Release();
                    break;
                case 1:
                    Console.WriteLine("Посредник кладет на стол табак и спички.");
                    tobacco.Release();
                    matches.Release();
                    break;
                case 2:
                    Console.WriteLine("Посредник кладет на стол табак и бумагу.");
                    tobacco.Release();
                    paper.Release();
                    break;
            }
        }
    }

    static void SmokerWithTobacco()
    {
        while (true)
        {
            paper.WaitOne();
            matches.WaitOne();
            Console.WriteLine("Курильщик с табаком курит.");
            Thread.Sleep(100);
            agent.Release();
        }
    }

    static void SmokerWithPaper()
    {
        while (true)
        {
            tobacco.WaitOne();
            matches.WaitOne();
            Console.WriteLine("Курильщик с бумагой курит.");
            Thread.Sleep(100);
            agent.Release();
        }
    }

    static void SmokerWithMatches()
    {
        while (true)
        {
            tobacco.WaitOne();
            paper.WaitOne();
            Console.WriteLine("Курильщик со спичками курит.");
            Thread.Sleep(100);
            agent.Release();
        }
    }
}
