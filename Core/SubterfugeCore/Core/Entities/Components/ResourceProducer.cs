﻿using System;
using Subterfuge.Remake.Core.GameEvents.Combat.CombatEvents;
using Subterfuge.Remake.Core.GameEvents.EventPublishers;
using Subterfuge.Remake.Core.Timing;

namespace Subterfuge.Remake.Core.Entities.Components
{
    public class ResourceProducer : IResourceProductionEventPublisher
    {
        private GameTick _nextProductionTick;
        private int _ticksPerProductionCycle;
        private int _nextValueToProduce;
        private ProducedResourceType _typeToProduce;
        private bool _isPaused = false;
        private TimeMachine _timeMachine;
        private IEntity _productionLocation;
        
        public bool IgnoresCapacity { get; set; }
        
        public ResourceProducer(
            IEntity productionLocation,
            TimeMachine timeMachine,
            int ticksPerProductionCycle,
            int baseValuePerProduction,
            ProducedResourceType producedType
        )
        {
	        _productionLocation = productionLocation;
	        _ticksPerProductionCycle = ticksPerProductionCycle;
	        _nextValueToProduce = baseValuePerProduction;
	        this._nextProductionTick = timeMachine.GetCurrentTick().Advance(_ticksPerProductionCycle);
	        _typeToProduce = producedType;
	        _timeMachine = timeMachine;
	        
            timeMachine.OnTick += ProductionTickListener;
        }
        
         public void SetPaused(bool isPaused)
	    {
		    _isPaused = isPaused;
	    }

	    public void ChangeAmountProducedPerCycle(int delta)
	    {
		    _nextValueToProduce = Math.Max(0, _nextValueToProduce + delta);
	    }

	    public int GetAmountProducedPerCycle()
	    {
		    return _nextValueToProduce;
	    }

	    public void ChangeTicksPerProductionCycle(int delta)
	    {
		    _ticksPerProductionCycle = Math.Max(0, _ticksPerProductionCycle + delta);
	    }

	    public GameTick GetNextProductionTick()
	    {
		    return _nextProductionTick;
	    }

	    public int GetTicksPerProductionCycle()
	    {
		    return _ticksPerProductionCycle;
	    }

        private void ProductionTickListener(object sender, OnTickEventArgs tickEventArgs)
        {
	        if (tickEventArgs.Direction == TimeMachineDirection.FORWARD)
			{
				if (_isPaused)
				{
					_nextProductionTick = _nextProductionTick.Advance(1);
					return;
				}
				
				if (_nextProductionTick == tickEventArgs.CurrentTick)
				{
					var resourceProduction = new ResourceProductionEvent(
						tickEventArgs.CurrentTick,
						locationProducedAt: _productionLocation,
						producedType: _typeToProduce,
						_nextValueToProduce,
						IgnoresCapacity
					);
					
					_timeMachine.AddEvent(resourceProduction);

					var productionEvent = new ProductionEventArgs()
					{
						TickProducedAt = tickEventArgs.CurrentTick,
						Direction = tickEventArgs.Direction,
						NextProduction = _nextProductionTick,
						ProducedResourceType = _typeToProduce,
						TimeMachine = _timeMachine,
						ProductionEvent = resourceProduction
					};
					
					_nextProductionTick = _nextProductionTick.Advance(_ticksPerProductionCycle);
					OnResourceProduced?.Invoke(this, productionEvent);
				}
			}
	        else
	        {
		        if (_isPaused)
		        {
			        _nextProductionTick = _nextProductionTick.Rewind(1);
			        return;
		        }
	        }
		}

        public event EventHandler<ProductionEventArgs>? OnResourceProduced;
    }
}