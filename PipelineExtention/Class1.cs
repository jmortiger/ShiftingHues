using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft;
using Newtonsoft.Json.Linq;

using ShiftingHues;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content;

namespace PipelineExtention
{
	[ContentImporter(".json", DefaultProcessor = "SpriteSheetInfoProcessor", DisplayName = "SpriteSheetInfo Importer - J Mor")]
	public class SpriteSheetInfoImporter : ContentImporter<SpriteSheetJSON>
	{
		public override SpriteSheetJSON Import(string filename, ContentImporterContext context)
		{
			context.Logger.LogMessage("Importing JSON file: {0}", filename);

			using (var streamReader = new StreamReader(filename))
			{
				JObject o = JObject.Parse(streamReader.ReadToEnd());
				streamReader.Close();
				return o.ToObject<SpriteSheetJSON>();
			}
			//throw new NotImplementedException();
		}
	}

	[ContentProcessor(DisplayName = "SpriteSheetInfo Processor - J Mor")]
	public class SpriteSheetInfoProcessor : ContentProcessor<SpriteSheetJSON, FileFileData>
	{
		public override FileFileData Process(SpriteSheetJSON input, ContentProcessorContext context)
		{
			try
			{
				context.Logger.LogMessage("Processing SpriteSheetInfo file");
				var output = new FileFileData(JsonConvert.SerializeObject(input)); // Currently Just rewraps the json again.
				return output;
			}
			catch (Exception e)
			{
				context.Logger.LogMessage("Error {0}", e);
				throw;
			}
			//throw new NotImplementedException();
		}
	}

	[ContentTypeWriter]
	public class SpriteSheetInfoWriter : ContentTypeWriter<FileFileData>
	{
		protected override void Write(ContentWriter output, FileFileData value)
		{
			output.Write(value.info);
			//throw new NotImplementedException();
		}

		public override string GetRuntimeType(TargetPlatform targetPlatform)
		{
			return typeof(SpriteSheetJSON).AssemblyQualifiedName;
			//return base.GetRuntimeType(targetPlatform);
		}

		public override string GetRuntimeReader(TargetPlatform targetPlatform)
		{
			return "ShiftingHues.SpriteSheetInfoReader, ShiftingHues";
			//return $"{typeof(SpriteSheetInfoReader).FullName}, ShiftingHues";
			//throw new NotImplementedException();
		}
	}
}
