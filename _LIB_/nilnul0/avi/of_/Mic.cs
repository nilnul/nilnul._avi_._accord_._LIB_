using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nilnul.avi.of_
{
	/// <summary>
	/// sound from mic;anime from block of screen
	/// </summary>
	public class Mic
	{
		nilnul.fs.FolderI container;
		Stopwatch stopwatch = new Stopwatch();

		nilnul.sound.of_.mic_.ByNaudio soundRec;
		nilnul.img.anime.of_.screen_.accord_.Frames screenRec;

		System.Drawing.Rectangle block;
		int framesPerSecond;

		public nilnul.fs.address_.SpearI aviAddress;

		public Mic(System.Drawing.Rectangle block__
			,
			int framesPerSecond__,
			nilnul.fs.FolderI container
		)
		{
			this.block = block__;
			this.framesPerSecond = framesPerSecond__;
			this.container = container;
		}

		static nilnul.fs.FolderI _This()
		{

			//return nilnul.fs.folder_.tmp.denote_.ver_.next_.subIfNeed._CreateFolderX.Folder("AviOfMic");
			var parent= nilnul.fs.folder._EnsureX.Folder(
				 nilnul.fs.address_.shield_.BaseDir.Create_dirDenote(
					nilnul.fs.folder_._TmpX.Get().address.en
					,
					"AviOfMic"
				)
			);

			return nilnul.fs.folder.denote_.vered_.next_.subIfNeed._CreateFolderX.Folder( parent, nilnul.time_.datetime.phrase_.Full.Singleton.phrase());


		}

		public Mic(System.Drawing.Rectangle block__, int framesPerSecond__ = 10)
			: this(block__, framesPerSecond__, _This())
		{

		}

		public void start()
		{

			var soundFile = new nilnul.fs.address_.spear_.ParentDoc(container.address.en, "s.wav");

			//stopwatch.Start();
			/// a thread to record sound;
			///

			soundRec = new nilnul.sound.of_.mic_.ByNaudio(soundFile.ToString());
			screenRec = new img.anime.of_.screen_.accord_.Frames(block, framesPerSecond
				, new nilnul.fs.address_.spear_.ParentDoc(this.container.address.en, "_frames_").ToString()
				);

			//soundRec.startEvt();

			var t = new Task(

				() => soundRec.startEvt()

			);
			///a thread to record pics
			///

			var t1 = new	Task(
				() => screenRec.startEvt()
			);

			t.Start();
			t1.Start();

			Task.WaitAll(t, t1);
		}

		public void stop()
		{
			///not necessary as time is computed by sound
			stopwatch.Stop();


			soundRec.stopEvt();
			screenRec.stop();
			soundRec.wait();

			///get the time of the sound
			///
			var duration = nilnul.sound.duration_.ByNaudio.OfFile(soundRec.outputFilePath);
			///generate anime
			///

			var animeGenerator = new nilnul.img.anime.of_.parent_.Accord(
				screenRec.framesParentFolder
				, duration, block
			);

			animeGenerator.save();
			///merge sound and anime

			var combiner =
			 new nilnul.avi.of_.aniSound_.ByFfmpeg(
				new nilnul.fs.folder.denote_.mainVered_.appendSubIfNeed_.Next(this.container).address("combined.mp4")
			);
			combiner.combineVideoAndAudio(animeGenerator.animeAddress, soundRec.outputFilePath);


			var combined = combiner.finalName;
			aviAddress = combined;
			nilnul.fs.file.del_._RecyclableX.Del(

			soundRec.outputFilePath
			);
			nilnul.fs.folder.drop_._RecyclableX.Del(
			screenRec.framesParentFolder
			);
			//nilnul.fs.file.explore_._SelX.Vod(combined);




		}
	}
}
