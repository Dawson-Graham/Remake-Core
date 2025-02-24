﻿using Subterfuge.Remake.Core.Entities;
using Subterfuge.Remake.Core.Entities.Components;
using Subterfuge.Remake.Core.GameEvents.Base;
using Subterfuge.Remake.Core.Timing;

namespace Subterfuge.Remake.Core.GameEvents.Combat.CombatEvents
{
    public class CombatDrillersEffect : PositionalGameEvent
    {
        private readonly IEntity _combatant1;
        private readonly IEntity _combatant2;

        private int _preCombatDrillers1;
        private int _preCombatDrillers2;
        
        public CombatDrillersEffect(
            GameTick occursAt,
            IEntity combatant1,
            IEntity combatant2
        ) : base(occursAt, Priority.DRILLER_COMBAT, combatant1)
        {
            _combatant1 = combatant1;
            _combatant2 = combatant2;
        }

        public override bool ForwardAction(TimeMachine timeMachine)
        {
            _preCombatDrillers1 = _combatant1.GetComponent<DrillerCarrier>().GetDrillerCount();
            _preCombatDrillers2 = _combatant2.GetComponent<DrillerCarrier>().GetDrillerCount();
            _combatant1.GetComponent<DrillerCarrier>().AlterDrillers(_preCombatDrillers2 * -1);
            _combatant2.GetComponent<DrillerCarrier>().AlterDrillers(_preCombatDrillers1 * -1);
            return true;
        }

        public override bool BackwardAction(TimeMachine timeMachine)
        {
            _combatant1.GetComponent<DrillerCarrier>().SetDrillerCount(_preCombatDrillers1);
            _combatant1.GetComponent<DrillerCarrier>().SetDrillerCount(_preCombatDrillers2);
            return true;
        }

        public override bool WasEventSuccessful()
        {
            return true;
        }
    }
}