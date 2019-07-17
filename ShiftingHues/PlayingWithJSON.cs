using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace ShiftingHues
{
	//[ContentImporter(".json", DefaultProcessor = "SpriteSheetInfoProcessor", DisplayName = "SpriteSheetInfo Importer - J Mor")]
	//public class SpriteSheetInfoImporter : ContentImporter<SpriteSheetJSON>
	//{
	//	public override SpriteSheetJSON Import(string filename, ContentImporterContext context)
	//	{
	//		context.Logger.LogMessage("Importing JSON file: {0}", filename);

	//		using (var streamReader = new StreamReader(filename))
	//		{
	//			JObject o = JObject.Parse(streamReader.ReadToEnd());
	//			streamReader.Close();
	//			return o.ToObject<SpriteSheetJSON>();
	//		}
	//		//throw new NotImplementedException();
	//	}
	//}

	//[ContentProcessor(DisplayName = "SpriteSheetInfo Processor - J Mor")]
	//public class SpriteSheetInfoProcessor : ContentProcessor<SpriteSheetJSON, FileFileData>
	//{
	//	public override FileFileData Process(SpriteSheetJSON input, ContentProcessorContext context)
	//	{
	//		try
	//		{
	//			context.Logger.LogMessage("Processing SpriteSheetInfo file");
	//			var output = new FileFileData(JsonConvert.SerializeObject(input)); // Currently Just rewraps the json again.
	//			return output;
	//		}
	//		catch (Exception e)
	//		{
	//			context.Logger.LogMessage("Error {0}", e);
	//			throw;
	//		}
	//		//throw new NotImplementedException();
	//	}
	//}

	//[ContentTypeWriter]
	//public class SpriteSheetInfoWriter : ContentTypeWriter<FileFileData>
	//{
	//	protected override void Write(ContentWriter output, FileFileData value)
	//	{
	//		output.Write(value.info);
	//		//throw new NotImplementedException();
	//	}

	//	public override string GetRuntimeType(TargetPlatform targetPlatform)
	//	{
	//		return typeof(SpriteSheetJSON).AssemblyQualifiedName;
	//		//return base.GetRuntimeType(targetPlatform);
	//	}

	//	public override string GetRuntimeReader(TargetPlatform targetPlatform)
	//	{
	//		return "ShiftingHues.SpriteSheetInfoReader, ShiftingHues";
	//		//return $"{typeof(SpriteSheetInfoReader).FullName}, ShiftingHues";
	//		//throw new NotImplementedException();
	//	}
	//}

	public class SpriteSheetInfoReader : ContentTypeReader<SpriteSheetJSON>
	{
		protected override SpriteSheetJSON Read(ContentReader input, SpriteSheetJSON existingInstance)
		{
			return JsonConvert.DeserializeObject<SpriteSheetJSON>(input.ReadString());
			//throw new NotImplementedException();
		}
	}

	public class FileFileData
	{
		public string info;
		public FileFileData(string info)
		{
			this.info = info;
		}
	}

	class PlayingWithJSON
	{
		#region Fields and Properties

		#endregion

		#region Constructors

		public PlayingWithJSON(string filePath)
		{
			////StreamReader streamReader = new StreamReader(filePath);
			//FileStream fileStream = File.OpenRead(filePath)/*new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)*/;
			//BinaryFormatter formatter = new BinaryFormatter();

			//SpriteSheetJSON sheetJSON = (SpriteSheetJSON)formatter.Deserialize(fileStream);
			//fileStream.Close();

			JObject o = JObject.Parse(File.ReadAllText(filePath));
			//JProperty spritesProp = o.Property("sprites");
			//List<SpriteJSON> spriteJSONs = new List<SpriteJSON>();
			//JToken token = spritesProp.Value;
			//JArray jArray = (JArray)token;
			//foreach (var jesuschrist in jArray)
			//{
			//	spriteJSONs.Add(jesuschrist.ToObject<SpriteJSON>());
			//}
			SpriteSheetJSON sheetJSON = o.ToObject<SpriteSheetJSON>();
		}
		#endregion
	}

	[JsonObject]
	[Serializable]
	public class SpriteSheetJSON : ISerializable
	{
		public string resourceID;
		public string textureFilePath;
		public SpriteJSON[] sprites;

		public SpriteSheetJSON()
		{

		}

		[JsonConstructor]
		public SpriteSheetJSON(string resourceID, string textureFilePath, SpriteJSON[] sprites)
		{
			this.resourceID = resourceID;
			this.textureFilePath = textureFilePath;
			this.sprites = sprites;
		}

		public SpriteSheetJSON(SerializationInfo info, StreamingContext context)
		{
			resourceID = info.GetString("resourceID");
			textureFilePath = info.GetString("textureFilePath");
			sprites = (SpriteJSON[])info.GetValue("sprites", typeof(SpriteJSON[]));
		}

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("resourceID", resourceID);
			info.AddValue("textureFilePath", textureFilePath);
			info.AddValue("sprites", sprites);
			//throw new NotImplementedException();
		}

		public Rectangle[] GetSourceRectangles()
		{
			Rectangle[] rectangles = new Rectangle[sprites.Length];
			for (int i = 0; i < sprites.Length; i++)
			{
				rectangles[i] = sprites[i].sourceRect.GetFRectangle().Rectangle;
			}
			return rectangles;
		}

		//public SpriteSheet GetSpriteSheet() => new SpriteSheet()
	}
	[Serializable]
	public class SpriteJSON : ISerializable
	{
		public string resourceID;
		public string[] animationsIncludedIn;
		public RectangleJSON sourceRect;

		public SpriteJSON()
		{

		}

		[JsonConstructor]
		public SpriteJSON(string resourceID, string[] animationsIncludedIn, RectangleJSON sourceRect)
		{
			this.resourceID = resourceID;
			this.animationsIncludedIn = animationsIncludedIn;
			this.sourceRect = sourceRect;
		}

		public SpriteJSON(SerializationInfo info, StreamingContext context)
		{
			resourceID = info.GetString("resourceID");
			animationsIncludedIn = (string[])info.GetValue("animationsIncludedIn", typeof(string[]));
			sourceRect = (RectangleJSON)info.GetValue("sourceRect", typeof(RectangleJSON));
		}

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("resourceID", resourceID);
			info.AddValue("animationsIncludedIn", animationsIncludedIn);
			info.AddValue("sourceRect", sourceRect);
			//throw new NotImplementedException();
		}
	}
	[Serializable]
	public class RectangleJSON : ISerializable
	{
		public int X;
		public int Y;
		public int Width;
		public int Height;

		public RectangleJSON()
		{

		}

		[JsonConstructor]
		public RectangleJSON(int X, int Y, int Width, int Height)
		{
			this.X = X;
			this.Y = Y;
			this.Width = Width;
			this.Height = Height;
		}

		public RectangleJSON(SerializationInfo info, StreamingContext context)
		{
			X = info.GetInt32("X");
			Y = info.GetInt32("Y");
			Width = info.GetInt32("Width");
			Height = info.GetInt32("Height");
		}

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("X", X);
			info.AddValue("Y", Y);
			info.AddValue("Width", Width);
			info.AddValue("Height", Height);
			//throw new NotImplementedException();
		}

		public FRectangle GetFRectangle() => new FRectangle(X, Y, Width, Height);
	}
}