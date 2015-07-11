// automatically generated, do not modify

namespace Arcinect.Schema
{

using FlatBuffers;

public sealed class FrameTimeline : Table {
  public static FrameTimeline GetRootAsFrameTimeline(ByteBuffer _bb) { return GetRootAsFrameTimeline(_bb, new FrameTimeline()); }
  public static FrameTimeline GetRootAsFrameTimeline(ByteBuffer _bb, FrameTimeline obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public static bool FrameTimelineBufferHasIdentifier(ByteBuffer _bb) { return __has_identifier(_bb, "ARCI"); }
  public FrameTimeline __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public FrameData GetFrames(int j) { return GetFrames(new FrameData(), j); }
  public FrameData GetFrames(FrameData obj, int j) { int o = __offset(4); return o != 0 ? obj.__init(__indirect(__vector(o) + j * 4), bb) : null; }
  public int FramesLength { get { int o = __offset(4); return o != 0 ? __vector_len(o) : 0; } }

  public static int CreateFrameTimeline(FlatBufferBuilder builder,
      int frames = 0) {
    builder.StartObject(1);
    FrameTimeline.AddFrames(builder, frames);
    return FrameTimeline.EndFrameTimeline(builder);
  }

  public static void StartFrameTimeline(FlatBufferBuilder builder) { builder.StartObject(1); }
  public static void AddFrames(FlatBufferBuilder builder, int framesOffset) { builder.AddOffset(0, framesOffset, 0); }
  public static int CreateFramesVector(FlatBufferBuilder builder, int[] data) { builder.StartVector(4, data.Length, 4); for (int i = data.Length - 1; i >= 0; i--) builder.AddOffset(data[i]); return builder.EndVector(); }
  public static void StartFramesVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(4, numElems, 4); }
  public static int EndFrameTimeline(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return o;
  }
  public static void FinishFrameTimelineBuffer(FlatBufferBuilder builder, int offset) { builder.Finish(offset, "ARCI"); }
};


}
