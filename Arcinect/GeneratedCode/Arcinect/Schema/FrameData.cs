// automatically generated, do not modify

namespace Arcinect.Schema
{

using FlatBuffers;

public sealed class FrameData : Table {
  public static FrameData GetRootAsFrameData(ByteBuffer _bb) { return GetRootAsFrameData(_bb, new FrameData()); }
  public static FrameData GetRootAsFrameData(ByteBuffer _bb, FrameData obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public FrameData __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public uint Time { get { int o = __offset(4); return o != 0 ? bb.GetUint(o + bb_pos) : (uint)0; } }
  public byte GetColorData(int j) { int o = __offset(6); return o != 0 ? bb.Get(__vector(o) + j * 1) : (byte)0; }
  public int ColorDataLength { get { int o = __offset(6); return o != 0 ? __vector_len(o) : 0; } }
  public ushort GetDepthData(int j) { int o = __offset(8); return o != 0 ? bb.GetUshort(__vector(o) + j * 2) : (ushort)0; }
  public int DepthDataLength { get { int o = __offset(8); return o != 0 ? __vector_len(o) : 0; } }

  public static int CreateFrameData(FlatBufferBuilder builder,
      uint time = 0,
      int colorData = 0,
      int depthData = 0) {
    builder.StartObject(3);
    FrameData.AddDepthData(builder, depthData);
    FrameData.AddColorData(builder, colorData);
    FrameData.AddTime(builder, time);
    return FrameData.EndFrameData(builder);
  }

  public static void StartFrameData(FlatBufferBuilder builder) { builder.StartObject(3); }
  public static void AddTime(FlatBufferBuilder builder, uint time) { builder.AddUint(0, time, 0); }
  public static void AddColorData(FlatBufferBuilder builder, int colorDataOffset) { builder.AddOffset(1, colorDataOffset, 0); }
  public static int CreateColorDataVector(FlatBufferBuilder builder, byte[] data) { builder.StartVector(1, data.Length, 1); for (int i = data.Length - 1; i >= 0; i--) builder.AddByte(data[i]); return builder.EndVector(); }
  public static void StartColorDataVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(1, numElems, 1); }
  public static void AddDepthData(FlatBufferBuilder builder, int depthDataOffset) { builder.AddOffset(2, depthDataOffset, 0); }
  public static int CreateDepthDataVector(FlatBufferBuilder builder, ushort[] data) { builder.StartVector(2, data.Length, 2); for (int i = data.Length - 1; i >= 0; i--) builder.AddUshort(data[i]); return builder.EndVector(); }
  public static void StartDepthDataVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(2, numElems, 2); }
  public static int EndFrameData(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return o;
  }
  public static void FinishFrameDataBuffer(FlatBufferBuilder builder, int offset) { builder.Finish(offset); }
};


}
