// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: Push.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Com.Violet.Rpc {

  /// <summary>Holder for reflection information generated from Push.proto</summary>
  public static partial class PushReflection {

    #region Descriptor
    /// <summary>File descriptor for Push.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static PushReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "CgpQdXNoLnByb3RvEg5jb20udmlvbGV0LnJwYxoMU3RydWN0LnByb3RvIkUK",
            "E1Jvb21TdGF0ZUNoYW5nZVB1c2gSLgoKcGxheWVyTGlzdBgBIAMoCzIaLmNv",
            "bS52aW9sZXQucnBjLlBsYXllckluZm8iwQEKFEVudGVyQmF0dGxlRmllbGRQ",
            "dXNoEi4KCnBsYXllckxpc3QYASADKAsyGi5jb20udmlvbGV0LnJwYy5QbGF5",
            "ZXJJbmZvEhIKCnJvdW5kT3JkZXIYAiABKAUSLwoMY2hlc3NTZXR0aW5nGAMg",
            "AygLMhkuY29tLnZpb2xldC5ycGMuQ2hlc3NEYXRhEg4KBnJvb21JZBgEIAEo",
            "CRIRCglyZWFkeVRpbWUYBSABKAUSEQoJcm91bmRUaW1lGAYgASgFIkcKFVBs",
            "YXllclN0YXRlQ2hhbmdlUHVzaBIuCgpwbGF5ZXJJbmZvGAEgASgLMhouY29t",
            "LnZpb2xldC5ycGMuUGxheWVySW5mbyJyChNHYW1lU3RhdGVDaGFuZ2VQdXNo",
            "Eg0KBXN0YXRlGAEgASgFEg4KBnJlc3VsdBgCIAEoBRIrCghjaGVzc01hcBgD",
            "IAMoCzIZLmNvbS52aW9sZXQucnBjLkNoZXNzRGF0YRIPCgdjb3VudGVyGAQg",
            "ASgFIr0BCg1DaGVzc01vdmVQdXNoEikKBnNvdXJjZRgBIAEoCzIZLmNvbS52",
            "aW9sZXQucnBjLkNoZXNzRGF0YRIpCgZ0YXJnZXQYAiABKAsyGS5jb20udmlv",
            "bGV0LnJwYy5DaGVzc0RhdGESFwoPY2hlc3NNb3ZlUmVzdWx0GAMgASgFEg8K",
            "B2NvdW50ZXIYBCABKAUSLAoIb3BlcmF0b3IYBSABKAsyGi5jb20udmlvbGV0",
            "LnJwYy5QbGF5ZXJJbmZvIk4KD0NoYXRNZXNzYWdlUHVzaBIRCglmcm9tV2hl",
            "cmUYASABKAUSKAoDbXNnGAIgASgLMhsuY29tLnZpb2xldC5ycGMuTWVzc2Fn",
            "ZURhdGFiBnByb3RvMw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Com.Violet.Rpc.StructReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Violet.Rpc.RoomStateChangePush), global::Com.Violet.Rpc.RoomStateChangePush.Parser, new[]{ "PlayerList" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Violet.Rpc.EnterBattleFieldPush), global::Com.Violet.Rpc.EnterBattleFieldPush.Parser, new[]{ "PlayerList", "RoundOrder", "ChessSetting", "RoomId", "ReadyTime", "RoundTime" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Violet.Rpc.PlayerStateChangePush), global::Com.Violet.Rpc.PlayerStateChangePush.Parser, new[]{ "PlayerInfo" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Violet.Rpc.GameStateChangePush), global::Com.Violet.Rpc.GameStateChangePush.Parser, new[]{ "State", "Result", "ChessMap", "Counter" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Violet.Rpc.ChessMovePush), global::Com.Violet.Rpc.ChessMovePush.Parser, new[]{ "Source", "Target", "ChessMoveResult", "Counter", "Operator" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Violet.Rpc.ChatMessagePush), global::Com.Violet.Rpc.ChatMessagePush.Parser, new[]{ "FromWhere", "Msg" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  ///房间状态发生变化，对方退出
  /// </summary>
  public sealed partial class RoomStateChangePush : pb::IMessage<RoomStateChangePush> {
    private static readonly pb::MessageParser<RoomStateChangePush> _parser = new pb::MessageParser<RoomStateChangePush>(() => new RoomStateChangePush());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<RoomStateChangePush> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Violet.Rpc.PushReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public RoomStateChangePush() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public RoomStateChangePush(RoomStateChangePush other) : this() {
      playerList_ = other.playerList_.Clone();
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public RoomStateChangePush Clone() {
      return new RoomStateChangePush(this);
    }

    /// <summary>Field number for the "playerList" field.</summary>
    public const int PlayerListFieldNumber = 1;
    private static readonly pb::FieldCodec<global::Com.Violet.Rpc.PlayerInfo> _repeated_playerList_codec
        = pb::FieldCodec.ForMessage(10, global::Com.Violet.Rpc.PlayerInfo.Parser);
    private readonly pbc::RepeatedField<global::Com.Violet.Rpc.PlayerInfo> playerList_ = new pbc::RepeatedField<global::Com.Violet.Rpc.PlayerInfo>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Com.Violet.Rpc.PlayerInfo> PlayerList {
      get { return playerList_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as RoomStateChangePush);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(RoomStateChangePush other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if(!playerList_.Equals(other.playerList_)) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      hash ^= playerList_.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      playerList_.WriteTo(output, _repeated_playerList_codec);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      size += playerList_.CalculateSize(_repeated_playerList_codec);
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(RoomStateChangePush other) {
      if (other == null) {
        return;
      }
      playerList_.Add(other.playerList_);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 10: {
            playerList_.AddEntriesFrom(input, _repeated_playerList_codec);
            break;
          }
        }
      }
    }

  }

  public sealed partial class EnterBattleFieldPush : pb::IMessage<EnterBattleFieldPush> {
    private static readonly pb::MessageParser<EnterBattleFieldPush> _parser = new pb::MessageParser<EnterBattleFieldPush>(() => new EnterBattleFieldPush());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<EnterBattleFieldPush> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Violet.Rpc.PushReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public EnterBattleFieldPush() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public EnterBattleFieldPush(EnterBattleFieldPush other) : this() {
      playerList_ = other.playerList_.Clone();
      roundOrder_ = other.roundOrder_;
      chessSetting_ = other.chessSetting_.Clone();
      roomId_ = other.roomId_;
      readyTime_ = other.readyTime_;
      roundTime_ = other.roundTime_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public EnterBattleFieldPush Clone() {
      return new EnterBattleFieldPush(this);
    }

    /// <summary>Field number for the "playerList" field.</summary>
    public const int PlayerListFieldNumber = 1;
    private static readonly pb::FieldCodec<global::Com.Violet.Rpc.PlayerInfo> _repeated_playerList_codec
        = pb::FieldCodec.ForMessage(10, global::Com.Violet.Rpc.PlayerInfo.Parser);
    private readonly pbc::RepeatedField<global::Com.Violet.Rpc.PlayerInfo> playerList_ = new pbc::RepeatedField<global::Com.Violet.Rpc.PlayerInfo>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Com.Violet.Rpc.PlayerInfo> PlayerList {
      get { return playerList_; }
    }

    /// <summary>Field number for the "roundOrder" field.</summary>
    public const int RoundOrderFieldNumber = 2;
    private int roundOrder_;
    /// <summary>
    ///回合顺序位置，从0开始，0先手
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int RoundOrder {
      get { return roundOrder_; }
      set {
        roundOrder_ = value;
      }
    }

    /// <summary>Field number for the "chessSetting" field.</summary>
    public const int ChessSettingFieldNumber = 3;
    private static readonly pb::FieldCodec<global::Com.Violet.Rpc.ChessData> _repeated_chessSetting_codec
        = pb::FieldCodec.ForMessage(26, global::Com.Violet.Rpc.ChessData.Parser);
    private readonly pbc::RepeatedField<global::Com.Violet.Rpc.ChessData> chessSetting_ = new pbc::RepeatedField<global::Com.Violet.Rpc.ChessData>();
    /// <summary>
    ///我方上次的摆子设置
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Com.Violet.Rpc.ChessData> ChessSetting {
      get { return chessSetting_; }
    }

    /// <summary>Field number for the "roomId" field.</summary>
    public const int RoomIdFieldNumber = 4;
    private string roomId_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string RoomId {
      get { return roomId_; }
      set {
        roomId_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "readyTime" field.</summary>
    public const int ReadyTimeFieldNumber = 5;
    private int readyTime_;
    /// <summary>
    ///准备的时间
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int ReadyTime {
      get { return readyTime_; }
      set {
        readyTime_ = value;
      }
    }

    /// <summary>Field number for the "roundTime" field.</summary>
    public const int RoundTimeFieldNumber = 6;
    private int roundTime_;
    /// <summary>
    ///每回合时间
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int RoundTime {
      get { return roundTime_; }
      set {
        roundTime_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as EnterBattleFieldPush);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(EnterBattleFieldPush other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if(!playerList_.Equals(other.playerList_)) return false;
      if (RoundOrder != other.RoundOrder) return false;
      if(!chessSetting_.Equals(other.chessSetting_)) return false;
      if (RoomId != other.RoomId) return false;
      if (ReadyTime != other.ReadyTime) return false;
      if (RoundTime != other.RoundTime) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      hash ^= playerList_.GetHashCode();
      if (RoundOrder != 0) hash ^= RoundOrder.GetHashCode();
      hash ^= chessSetting_.GetHashCode();
      if (RoomId.Length != 0) hash ^= RoomId.GetHashCode();
      if (ReadyTime != 0) hash ^= ReadyTime.GetHashCode();
      if (RoundTime != 0) hash ^= RoundTime.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      playerList_.WriteTo(output, _repeated_playerList_codec);
      if (RoundOrder != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(RoundOrder);
      }
      chessSetting_.WriteTo(output, _repeated_chessSetting_codec);
      if (RoomId.Length != 0) {
        output.WriteRawTag(34);
        output.WriteString(RoomId);
      }
      if (ReadyTime != 0) {
        output.WriteRawTag(40);
        output.WriteInt32(ReadyTime);
      }
      if (RoundTime != 0) {
        output.WriteRawTag(48);
        output.WriteInt32(RoundTime);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      size += playerList_.CalculateSize(_repeated_playerList_codec);
      if (RoundOrder != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(RoundOrder);
      }
      size += chessSetting_.CalculateSize(_repeated_chessSetting_codec);
      if (RoomId.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(RoomId);
      }
      if (ReadyTime != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(ReadyTime);
      }
      if (RoundTime != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(RoundTime);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(EnterBattleFieldPush other) {
      if (other == null) {
        return;
      }
      playerList_.Add(other.playerList_);
      if (other.RoundOrder != 0) {
        RoundOrder = other.RoundOrder;
      }
      chessSetting_.Add(other.chessSetting_);
      if (other.RoomId.Length != 0) {
        RoomId = other.RoomId;
      }
      if (other.ReadyTime != 0) {
        ReadyTime = other.ReadyTime;
      }
      if (other.RoundTime != 0) {
        RoundTime = other.RoundTime;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 10: {
            playerList_.AddEntriesFrom(input, _repeated_playerList_codec);
            break;
          }
          case 16: {
            RoundOrder = input.ReadInt32();
            break;
          }
          case 26: {
            chessSetting_.AddEntriesFrom(input, _repeated_chessSetting_codec);
            break;
          }
          case 34: {
            RoomId = input.ReadString();
            break;
          }
          case 40: {
            ReadyTime = input.ReadInt32();
            break;
          }
          case 48: {
            RoundTime = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///敌人状态改变
  /// </summary>
  public sealed partial class PlayerStateChangePush : pb::IMessage<PlayerStateChangePush> {
    private static readonly pb::MessageParser<PlayerStateChangePush> _parser = new pb::MessageParser<PlayerStateChangePush>(() => new PlayerStateChangePush());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<PlayerStateChangePush> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Violet.Rpc.PushReflection.Descriptor.MessageTypes[2]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public PlayerStateChangePush() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public PlayerStateChangePush(PlayerStateChangePush other) : this() {
      PlayerInfo = other.playerInfo_ != null ? other.PlayerInfo.Clone() : null;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public PlayerStateChangePush Clone() {
      return new PlayerStateChangePush(this);
    }

    /// <summary>Field number for the "playerInfo" field.</summary>
    public const int PlayerInfoFieldNumber = 1;
    private global::Com.Violet.Rpc.PlayerInfo playerInfo_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Com.Violet.Rpc.PlayerInfo PlayerInfo {
      get { return playerInfo_; }
      set {
        playerInfo_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as PlayerStateChangePush);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(PlayerStateChangePush other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!object.Equals(PlayerInfo, other.PlayerInfo)) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (playerInfo_ != null) hash ^= PlayerInfo.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (playerInfo_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(PlayerInfo);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (playerInfo_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(PlayerInfo);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(PlayerStateChangePush other) {
      if (other == null) {
        return;
      }
      if (other.playerInfo_ != null) {
        if (playerInfo_ == null) {
          playerInfo_ = new global::Com.Violet.Rpc.PlayerInfo();
        }
        PlayerInfo.MergeFrom(other.PlayerInfo);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 10: {
            if (playerInfo_ == null) {
              playerInfo_ = new global::Com.Violet.Rpc.PlayerInfo();
            }
            input.ReadMessage(playerInfo_);
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///游戏状态改变：布子，布子完成，游戏结束
  /// </summary>
  public sealed partial class GameStateChangePush : pb::IMessage<GameStateChangePush> {
    private static readonly pb::MessageParser<GameStateChangePush> _parser = new pb::MessageParser<GameStateChangePush>(() => new GameStateChangePush());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<GameStateChangePush> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Violet.Rpc.PushReflection.Descriptor.MessageTypes[3]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public GameStateChangePush() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public GameStateChangePush(GameStateChangePush other) : this() {
      state_ = other.state_;
      result_ = other.result_;
      chessMap_ = other.chessMap_.Clone();
      counter_ = other.counter_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public GameStateChangePush Clone() {
      return new GameStateChangePush(this);
    }

    /// <summary>Field number for the "state" field.</summary>
    public const int StateFieldNumber = 1;
    private int state_;
    /// <summary>
    ///enum GameState
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int State {
      get { return state_; }
      set {
        state_ = value;
      }
    }

    /// <summary>Field number for the "result" field.</summary>
    public const int ResultFieldNumber = 2;
    private int result_;
    /// <summary>
    ///enum GameResult
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Result {
      get { return result_; }
      set {
        result_ = value;
      }
    }

    /// <summary>Field number for the "chessMap" field.</summary>
    public const int ChessMapFieldNumber = 3;
    private static readonly pb::FieldCodec<global::Com.Violet.Rpc.ChessData> _repeated_chessMap_codec
        = pb::FieldCodec.ForMessage(26, global::Com.Violet.Rpc.ChessData.Parser);
    private readonly pbc::RepeatedField<global::Com.Violet.Rpc.ChessData> chessMap_ = new pbc::RepeatedField<global::Com.Violet.Rpc.ChessData>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Com.Violet.Rpc.ChessData> ChessMap {
      get { return chessMap_; }
    }

    /// <summary>Field number for the "counter" field.</summary>
    public const int CounterFieldNumber = 4;
    private int counter_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Counter {
      get { return counter_; }
      set {
        counter_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as GameStateChangePush);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(GameStateChangePush other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (State != other.State) return false;
      if (Result != other.Result) return false;
      if(!chessMap_.Equals(other.chessMap_)) return false;
      if (Counter != other.Counter) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (State != 0) hash ^= State.GetHashCode();
      if (Result != 0) hash ^= Result.GetHashCode();
      hash ^= chessMap_.GetHashCode();
      if (Counter != 0) hash ^= Counter.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (State != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(State);
      }
      if (Result != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(Result);
      }
      chessMap_.WriteTo(output, _repeated_chessMap_codec);
      if (Counter != 0) {
        output.WriteRawTag(32);
        output.WriteInt32(Counter);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (State != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(State);
      }
      if (Result != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Result);
      }
      size += chessMap_.CalculateSize(_repeated_chessMap_codec);
      if (Counter != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Counter);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(GameStateChangePush other) {
      if (other == null) {
        return;
      }
      if (other.State != 0) {
        State = other.State;
      }
      if (other.Result != 0) {
        Result = other.Result;
      }
      chessMap_.Add(other.chessMap_);
      if (other.Counter != 0) {
        Counter = other.Counter;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 8: {
            State = input.ReadInt32();
            break;
          }
          case 16: {
            Result = input.ReadInt32();
            break;
          }
          case 26: {
            chessMap_.AddEntriesFrom(input, _repeated_chessMap_codec);
            break;
          }
          case 32: {
            Counter = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  public sealed partial class ChessMovePush : pb::IMessage<ChessMovePush> {
    private static readonly pb::MessageParser<ChessMovePush> _parser = new pb::MessageParser<ChessMovePush>(() => new ChessMovePush());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<ChessMovePush> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Violet.Rpc.PushReflection.Descriptor.MessageTypes[4]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ChessMovePush() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ChessMovePush(ChessMovePush other) : this() {
      Source = other.source_ != null ? other.Source.Clone() : null;
      Target = other.target_ != null ? other.Target.Clone() : null;
      chessMoveResult_ = other.chessMoveResult_;
      counter_ = other.counter_;
      Operator = other.operator_ != null ? other.Operator.Clone() : null;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ChessMovePush Clone() {
      return new ChessMovePush(this);
    }

    /// <summary>Field number for the "source" field.</summary>
    public const int SourceFieldNumber = 1;
    private global::Com.Violet.Rpc.ChessData source_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Com.Violet.Rpc.ChessData Source {
      get { return source_; }
      set {
        source_ = value;
      }
    }

    /// <summary>Field number for the "target" field.</summary>
    public const int TargetFieldNumber = 2;
    private global::Com.Violet.Rpc.ChessData target_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Com.Violet.Rpc.ChessData Target {
      get { return target_; }
      set {
        target_ = value;
      }
    }

    /// <summary>Field number for the "chessMoveResult" field.</summary>
    public const int ChessMoveResultFieldNumber = 3;
    private int chessMoveResult_;
    /// <summary>
    ///enum ChessMoveResult
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int ChessMoveResult {
      get { return chessMoveResult_; }
      set {
        chessMoveResult_ = value;
      }
    }

    /// <summary>Field number for the "counter" field.</summary>
    public const int CounterFieldNumber = 4;
    private int counter_;
    /// <summary>
    ///计数器
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Counter {
      get { return counter_; }
      set {
        counter_ = value;
      }
    }

    /// <summary>Field number for the "operator" field.</summary>
    public const int OperatorFieldNumber = 5;
    private global::Com.Violet.Rpc.PlayerInfo operator_;
    /// <summary>
    ///走子的人
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Com.Violet.Rpc.PlayerInfo Operator {
      get { return operator_; }
      set {
        operator_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as ChessMovePush);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(ChessMovePush other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!object.Equals(Source, other.Source)) return false;
      if (!object.Equals(Target, other.Target)) return false;
      if (ChessMoveResult != other.ChessMoveResult) return false;
      if (Counter != other.Counter) return false;
      if (!object.Equals(Operator, other.Operator)) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (source_ != null) hash ^= Source.GetHashCode();
      if (target_ != null) hash ^= Target.GetHashCode();
      if (ChessMoveResult != 0) hash ^= ChessMoveResult.GetHashCode();
      if (Counter != 0) hash ^= Counter.GetHashCode();
      if (operator_ != null) hash ^= Operator.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (source_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(Source);
      }
      if (target_ != null) {
        output.WriteRawTag(18);
        output.WriteMessage(Target);
      }
      if (ChessMoveResult != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(ChessMoveResult);
      }
      if (Counter != 0) {
        output.WriteRawTag(32);
        output.WriteInt32(Counter);
      }
      if (operator_ != null) {
        output.WriteRawTag(42);
        output.WriteMessage(Operator);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (source_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Source);
      }
      if (target_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Target);
      }
      if (ChessMoveResult != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(ChessMoveResult);
      }
      if (Counter != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Counter);
      }
      if (operator_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Operator);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(ChessMovePush other) {
      if (other == null) {
        return;
      }
      if (other.source_ != null) {
        if (source_ == null) {
          source_ = new global::Com.Violet.Rpc.ChessData();
        }
        Source.MergeFrom(other.Source);
      }
      if (other.target_ != null) {
        if (target_ == null) {
          target_ = new global::Com.Violet.Rpc.ChessData();
        }
        Target.MergeFrom(other.Target);
      }
      if (other.ChessMoveResult != 0) {
        ChessMoveResult = other.ChessMoveResult;
      }
      if (other.Counter != 0) {
        Counter = other.Counter;
      }
      if (other.operator_ != null) {
        if (operator_ == null) {
          operator_ = new global::Com.Violet.Rpc.PlayerInfo();
        }
        Operator.MergeFrom(other.Operator);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 10: {
            if (source_ == null) {
              source_ = new global::Com.Violet.Rpc.ChessData();
            }
            input.ReadMessage(source_);
            break;
          }
          case 18: {
            if (target_ == null) {
              target_ = new global::Com.Violet.Rpc.ChessData();
            }
            input.ReadMessage(target_);
            break;
          }
          case 24: {
            ChessMoveResult = input.ReadInt32();
            break;
          }
          case 32: {
            Counter = input.ReadInt32();
            break;
          }
          case 42: {
            if (operator_ == null) {
              operator_ = new global::Com.Violet.Rpc.PlayerInfo();
            }
            input.ReadMessage(operator_);
            break;
          }
        }
      }
    }

  }

  public sealed partial class ChatMessagePush : pb::IMessage<ChatMessagePush> {
    private static readonly pb::MessageParser<ChatMessagePush> _parser = new pb::MessageParser<ChatMessagePush>(() => new ChatMessagePush());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<ChatMessagePush> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Violet.Rpc.PushReflection.Descriptor.MessageTypes[5]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ChatMessagePush() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ChatMessagePush(ChatMessagePush other) : this() {
      fromWhere_ = other.fromWhere_;
      Msg = other.msg_ != null ? other.Msg.Clone() : null;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ChatMessagePush Clone() {
      return new ChatMessagePush(this);
    }

    /// <summary>Field number for the "fromWhere" field.</summary>
    public const int FromWhereFieldNumber = 1;
    private int fromWhere_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int FromWhere {
      get { return fromWhere_; }
      set {
        fromWhere_ = value;
      }
    }

    /// <summary>Field number for the "msg" field.</summary>
    public const int MsgFieldNumber = 2;
    private global::Com.Violet.Rpc.MessageData msg_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Com.Violet.Rpc.MessageData Msg {
      get { return msg_; }
      set {
        msg_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as ChatMessagePush);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(ChatMessagePush other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (FromWhere != other.FromWhere) return false;
      if (!object.Equals(Msg, other.Msg)) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (FromWhere != 0) hash ^= FromWhere.GetHashCode();
      if (msg_ != null) hash ^= Msg.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (FromWhere != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(FromWhere);
      }
      if (msg_ != null) {
        output.WriteRawTag(18);
        output.WriteMessage(Msg);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (FromWhere != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(FromWhere);
      }
      if (msg_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Msg);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(ChatMessagePush other) {
      if (other == null) {
        return;
      }
      if (other.FromWhere != 0) {
        FromWhere = other.FromWhere;
      }
      if (other.msg_ != null) {
        if (msg_ == null) {
          msg_ = new global::Com.Violet.Rpc.MessageData();
        }
        Msg.MergeFrom(other.Msg);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 8: {
            FromWhere = input.ReadInt32();
            break;
          }
          case 18: {
            if (msg_ == null) {
              msg_ = new global::Com.Violet.Rpc.MessageData();
            }
            input.ReadMessage(msg_);
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
