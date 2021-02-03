﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SubterfugeCore;
using System;
using SubterfugeCore.Core;
using SubterfugeCore.Core.Timing;

namespace SubterfugeCoreTest
{
    [TestClass]
    public class GameTickTest
    {
        DateTime _time;
        int _tickNumber;
        GameTick _tick;


        [TestInitialize]
        public void Setup()
        {
            _time = DateTime.Now;
            _tickNumber = 0;
            _tick = new GameTick(_time, _tickNumber);
            Game server = new Game();

        }

        [TestMethod]
        public void Constructor()
        {
            Assert.AreEqual(_tickNumber, _tick.GetTick());
            Assert.AreEqual(_time, _tick.GetDate());
        }

        [TestMethod]
        public void GetNextTick()
        {
            GameTick nextTick = _tick.GetNextTick();

            Assert.AreEqual(_time.AddMinutes(GameTick.MINUTES_PER_TICK), nextTick.GetDate());
            Assert.AreEqual(_tickNumber + 1, nextTick.GetTick());
        }

        [TestMethod]
        public void GetPreviousTick()
        {
            // Check cannot go back before the first tick
            GameTick previousTick = _tick.GetPreviousTick();

            Assert.AreEqual(_tick, previousTick);

            // Advance and then come back
            GameTick startingTick = _tick.GetNextTick().GetPreviousTick();

            Assert.AreEqual(_time, startingTick.GetDate());
            Assert.AreEqual(_tickNumber, startingTick.GetTick());
        }

        [TestMethod]
        public void Advance()
        {
            // Check advancing N ticks
            int ticksToAdvance = 10;
            GameTick tenMoreTicks = _tick.Advance(ticksToAdvance);

            Assert.AreEqual(_time.AddMinutes(GameTick.MINUTES_PER_TICK * ticksToAdvance), tenMoreTicks.GetDate());
            Assert.AreEqual(_tickNumber + ticksToAdvance, tenMoreTicks.GetTick());
        }

        [TestMethod]
        public void Rewind()
        {
            int ticksToReverse = 10;
            GameTick tenTicksBefore = _tick.Rewind(ticksToReverse);

            // Should not be able to reverse 
            Assert.AreEqual(true, tenTicksBefore == _tick);

            // Advance and then go back
            GameTick startingTick = _tick.Advance(ticksToReverse).Rewind(ticksToReverse);

            Assert.AreEqual(_time, startingTick.GetDate());
            Assert.AreEqual(_tickNumber, startingTick.GetTick());

        }

        [TestMethod]
        public void FromDate()
        {
            int numberOfTicks = 4;
            double minutes = GameTick.MINUTES_PER_TICK * numberOfTicks;
            DateTime newDate = _time.AddMinutes(minutes);
            GameTick newTick = GameTick.FromDate(newDate);

            Assert.AreEqual(_tick.GetTick() + numberOfTicks, newTick.GetTick());
            Assert.AreEqual(newDate.ToLongTimeString(), newTick.GetDate().ToLongTimeString());
            Assert.AreEqual(newDate.ToLongDateString(), newTick.GetDate().ToLongDateString());
        }

        [TestMethod]
        public void FromTick()
        {
            int numberOfTicks = 4;
            double minutes = GameTick.MINUTES_PER_TICK * numberOfTicks;
            DateTime newDate = _time.AddMinutes(minutes);
            GameTick newTick = GameTick.FromTickNumber(numberOfTicks);

            Assert.AreEqual(_tick.GetTick() + numberOfTicks, newTick.GetTick());
            Assert.AreEqual(newDate.ToLongTimeString(), newTick.GetDate().ToLongTimeString());
            Assert.AreEqual(newDate.ToLongDateString(), newTick.GetDate().ToLongDateString());
        }

        [TestMethod]
        public void FasterGameSpeed()
        {
            GameTick.MINUTES_PER_TICK = 0.1;
            DateTime start = _tick.GetDate();
            GameTick forward = _tick.Advance(10);
            Assert.AreEqual(forward.GetDate().ToLongTimeString(), start.AddMinutes(GameTick.MINUTES_PER_TICK * 10).ToLongTimeString());
        }

    }
}
