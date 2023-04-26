﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Core;
using YamlDotNet.Serialization.EventEmitters;
using YamlDotNet.Serialization;
using YamlDotNet.Core.Events;

namespace KubeUI.Core.Client.Serialization
{
    internal class FloatEmitter : ChainedEventEmitter
    {
        public FloatEmitter(IEventEmitter nextEmitter)
            : base(nextEmitter)
        {
        }

        public override void Emit(ScalarEventInfo eventInfo, IEmitter emitter)
        {
            switch (eventInfo.Source.Value)
            {
                // Floating point numbers should always render at least one zero (e.g. 1.0f => '1.0' not '1')
                case double d:
                    emitter.Emit(new Scalar(d.ToString("0.0######################")));
                    break;
                case float f:
                    emitter.Emit(new Scalar(f.ToString("0.0######################")));
                    break;
                default:
                    base.Emit(eventInfo, emitter);
                    break;
            }
        }
    }
}
