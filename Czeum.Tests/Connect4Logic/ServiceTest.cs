using System.Collections.Generic;
using Czeum.Abstractions;
using Czeum.Abstractions.DTO;
using Czeum.Abstractions.GameServices;
using Czeum.ChessLogic;
using Czeum.Connect4Logic;
using Czeum.DTO.Connect4;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Czeum.Tests.Connect4Logic
{
    [TestClass]
    public class ServiceTest
    {
        private Connect4MoveData move;
        private List<IGameService> services;
        private DummyRepository repository;

        [TestInitialize]
        public void Init()
        {
            move = new Connect4MoveData
            {
                Column = 0,
                MatchId = 1
            };

            repository = new DummyRepository();
            services = new List<IGameService>
            {
                new ChessService(null),
                new Connect4Service(repository)
            };
        }

        [TestMethod]
        public void MoveDataFindsService()
        {
            var service = move.FindGameService(services);
            Assert.AreSame(services[1], service);
        }

        [TestMethod]
        public void ExecutionTest()
        {
            var service = move.FindGameService(services);
            var result = (Connect4MoveResult) service.ExecuteMove(move, 1);
            
            Assert.AreEqual(Status.Success, result.Status);
            Assert.AreEqual(Item.Red, result.Board[result.Board.GetLength(0) - 1, 0]);
            Assert.IsTrue(repository.GetByMatchId(1).BoardData.Contains('R'));
        }
    }
}