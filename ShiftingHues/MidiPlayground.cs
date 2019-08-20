using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CoreMidi;
using Commons.Music.Midi;
using Commons.Music.Midi.WinMM;

using Sanford.Multimedia.Midi;
using Sanford.Multimedia;

namespace ShiftingHues
{
	public class MidiPlayground
	{
		public static void Test1()
		{
			//var access = MidiAccessManager.Default;
			//var output = access.OpenOutputAsync(access.Outputs.Last().Id).Result;
			////output.Send(new byte[] { 0xC0, GeneralMidi.Instruments.AcousticGrandPiano }, 0, 2, 0);
			////output.Send(new byte[] { MidiEvent.NoteOn, 0x40, 0x70 }, 0, 3, 0); // There are constant fields for each MIDI event
			////output.Send(new byte[] { MidiEvent.NoteOff, 0x40, 0x70 }, 0, 3, 0);
			////output.Send(new byte[] { MidiEvent.Program, 0x30 }, 0, 2, 0); // Strings Ensemble
			////output.Send(new byte[] { 0x90, 0x40, 0x70 }, 0, 3, 0);
			////output.Send(new byte[] { 0x80, 0x40, 0x70 }, 0, 3, 0);
			////output.CloseAsync();
			//var music = MidiMusic.Read(System.IO.File.OpenRead(@"C:\Users\jmort\Desktop\MIDIs\DOOM\level1.mid"));
			//var player = new MidiPlayer(music, output);
			//player.EventReceived += (MidiEvent e) =>
			//{
			//	if (e.EventType == MidiEvent.Program)
			//	{
			//		Console.WriteLine($"Program changed: Channel:{e.Channel} Instrument:{e.Msb}");
			//	}
			//};
			//player.Play();
			//Console.WriteLine("type cr to stop");
			//Console.ReadLine();
			//player.Dispose();


		}
	}
}
