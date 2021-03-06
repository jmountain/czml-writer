﻿using System;
using System.Drawing;
using Shapefile;
using CesiumLanguageWriter;

namespace ShapefileToCesiumLanguage
{
    public class MultiPoint : CzmlShape
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MultiPoint"/> class.
        /// </summary>
        /// <param name="multipoint">A MultiPointShape object read from a shapefile.</param>
        /// <param name="document">The CzmlDocument to write to.</param>
        /// <param name="color">The points's color.</param>
        public MultiPoint(MultiPointShape multipoint, CzmlDocument document, Color color)
            : base(document, color)
        {
            m_shape = multipoint;
        }


        public override void Write()
        {
            MultiPointShape multipoint = (MultiPointShape)m_shape;
            for (int i = 0; i < multipoint.Count; i++)
            {
                this.WritePacket(i);
            }
        }

        /// <summary>
        /// Writes each point to its <see cref="CzmlDocument"/> as a separate packet.
        /// </summary>
        private void WritePacket(int index)
        {
            using (PacketCesiumWriter packetWriter = m_document.CesiumStreamWriter.OpenPacket(m_document.CesiumOutputStream))
            {
                packetWriter.WriteId(Guid.NewGuid().ToString());
                MultiPointShape multipoint = (MultiPointShape)m_shape;
                using (PositionCesiumWriter position = packetWriter.OpenPositionProperty())
                {
                    position.WriteCartographicRadians(multipoint[index]);
                }

                using (PointCesiumWriter pointWriter = packetWriter.OpenPointProperty())
                {
                    pointWriter.WriteColorProperty(m_color);
                }
            }
        }
    }
}
