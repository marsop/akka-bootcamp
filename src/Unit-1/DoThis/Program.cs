using System;
﻿using Akka.Actor;

namespace WinTail
{
    #region Program
    class Program
    {
        public static ActorSystem MyActorSystem;

        static void Main(string[] args)
        {
            // initialize MyActorSystem
            MyActorSystem = ActorSystem.Create("MyActorSystem");

            Props tailCoordinatorProps = Props.Create(() => new TailCoordinatorActor());
            IActorRef tailCoordinatorActor = MyActorSystem.ActorOf(tailCoordinatorProps, nameof(TailCoordinatorActor));

            var consoleWriterActor = MyActorSystem.ActorOf(Props.Create(() => new ConsoleWriterActor()), nameof(ConsoleWriterActor));

            Props fileValidatorActorProps = Props.Create(() =>new FileValidatorActor(consoleWriterActor));
            IActorRef validationActor = MyActorSystem.ActorOf(fileValidatorActorProps, "validationActor");

            var consoleReaderActor = MyActorSystem.ActorOf(Props.Create(() => new ConsoleReaderActor()), nameof(ConsoleReaderActor));

            // tell console reader to begin
            consoleReaderActor.Tell(ConsoleReaderActor.StartCommand);

            // blocks the main thread from exiting until the actor system is shut down
            MyActorSystem.WhenTerminated.Wait();
        }
    }
    #endregion
}
