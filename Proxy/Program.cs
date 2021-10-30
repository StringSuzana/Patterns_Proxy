using System;

namespace Proxy
{
    public class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            p.do_stuff();
        }

        void do_stuff()
        {
            UserRepository repo = new ProxyUserRepository(new DefaultUserRepository());
            someFunction(repo);
        }
        void someFunction(UserRepository repo)
        {
            repo.updateAllBornAfter(2000);
        }

    }
    public class ProxyUserRepository : UserRepository
    {
        public DefaultUserRepository DefaultUserRepository { get; set; }
        private static int MAX_RETRY_COUNT { get => 10; }
        private int RETRY_COUNT { get; set; }
        public ProxyUserRepository(DefaultUserRepository default_repo)
        {
            DefaultUserRepository = default_repo;
        }
        public void updateAllBornAfter(int year)
        {
            for (int i = 0; i < MAX_RETRY_COUNT; i++)
            {
                Console.WriteLine(i);
                try
                {
                    DefaultUserRepository.updateAllBornAfter(year);
                    break;
                }
                catch (DeadlockException e)
                {

                   

                }
            }
        }

    }

    public interface UserRepository
    {
        public void updateAllBornAfter(int year);
    }
    public class DeadlockException : Exception { }
    public class DefaultUserRepository : UserRepository
    {
        public void updateAllBornAfter(int year)
        {
            if (Environment.TickCount % 2 == 0)
            {
                throw new DeadlockException();
            }
        }

    }

}
