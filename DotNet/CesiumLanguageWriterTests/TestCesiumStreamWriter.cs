﻿using System;
using System.IO;
using CesiumLanguageWriter;
using NUnit.Framework;

namespace CesiumLanguageWriterTests
{
    [TestFixture]
    public class TestCesiumStreamWriter
    {
        private StringWriter m_sw;
        private CesiumOutputStream m_output;
        private CesiumStreamWriter m_writer;

        [SetUp]
        public void SetUp()
        {
            m_sw = new StringWriter();
            m_output = new CesiumOutputStream(m_sw);
            m_writer = new CesiumStreamWriter();
        }

        [Test]
        public void NewPacketWritesObjectLiteral()
        {
            PacketCesiumWriter packet = m_writer.OpenPacket(m_output);
            packet.Close();

            Assert.AreEqual("{}", m_sw.ToString());
        }

        [Test]
        public void SuccessiveCallsToNewPacketReturnSameInstance()
        {
            m_output.WriteStartSequence();
            PacketCesiumWriter packet1 = m_writer.OpenPacket(m_output);
            packet1.Close();
            PacketCesiumWriter packet2 = m_writer.OpenPacket(m_output);
            packet2.Close();
            m_output.WriteEndSequence();
            Assert.AreSame(packet1, packet2);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "already opened", MatchType = MessageMatch.Contains)]
        public void MultipleCallsToNewPacketWithoutCloseThrowInvalidOperationException()
        {
            PacketCesiumWriter packet = m_writer.OpenPacket(m_output);
            packet = m_writer.OpenPacket(m_output);
        }
    }
}
