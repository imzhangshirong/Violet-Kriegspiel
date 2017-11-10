// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: Client.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Com.Violet.Rpc {

  /// <summary>Holder for reflection information generated from Client.proto</summary>
  public static partial class ClientReflection {

    #region Descriptor
    /// <summary>File descriptor for Client.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static ClientReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "CgxDbGllbnQucHJvdG8SDmNvbS52aW9sZXQucnBjIjQKCF9SZXF1ZXN0Eg0K",
            "BXRva2VuGAEgASgJEgsKA3JwYxgCIAEoCRIMCgRkYXRhGAMgASgMIkMKCV9S",
            "ZXNwb25zZRINCgV0b2tlbhgBIAEoCRIMCgRjb2RlGAIgASgFEgsKA3JwYxgD",
            "IAEoCRIMCgRkYXRhGAQgASgMIh8KDEhlbGxvUmVxdWVzdBIPCgdjb250ZW50",
            "GAEgASgJIh4KDUhlbGxvUmVzcG9uc2USDQoFZ3JlZXQYASABKAliBnByb3Rv",
            "Mw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Violet.Rpc._Request), global::Com.Violet.Rpc._Request.Parser, new[]{ "Token", "Rpc", "Data" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Violet.Rpc._Response), global::Com.Violet.Rpc._Response.Parser, new[]{ "Token", "Code", "Rpc", "Data" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Violet.Rpc.HelloRequest), global::Com.Violet.Rpc.HelloRequest.Parser, new[]{ "Content" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Violet.Rpc.HelloResponse), global::Com.Violet.Rpc.HelloResponse.Parser, new[]{ "Greet" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class _Request : pb::IMessage<_Request> {
    private static readonly pb::MessageParser<_Request> _parser = new pb::MessageParser<_Request>(() => new _Request());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<_Request> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Violet.Rpc.ClientReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public _Request() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public _Request(_Request other) : this() {
      token_ = other.token_;
      rpc_ = other.rpc_;
      data_ = other.data_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public _Request Clone() {
      return new _Request(this);
    }

    /// <summary>Field number for the "token" field.</summary>
    public const int TokenFieldNumber = 1;
    private string token_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Token {
      get { return token_; }
      set {
        token_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "rpc" field.</summary>
    public const int RpcFieldNumber = 2;
    private string rpc_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Rpc {
      get { return rpc_; }
      set {
        rpc_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "data" field.</summary>
    public const int DataFieldNumber = 3;
    private pb::ByteString data_ = pb::ByteString.Empty;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pb::ByteString Data {
      get { return data_; }
      set {
        data_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as _Request);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(_Request other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Token != other.Token) return false;
      if (Rpc != other.Rpc) return false;
      if (Data != other.Data) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Token.Length != 0) hash ^= Token.GetHashCode();
      if (Rpc.Length != 0) hash ^= Rpc.GetHashCode();
      if (Data.Length != 0) hash ^= Data.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Token.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Token);
      }
      if (Rpc.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(Rpc);
      }
      if (Data.Length != 0) {
        output.WriteRawTag(26);
        output.WriteBytes(Data);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Token.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Token);
      }
      if (Rpc.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Rpc);
      }
      if (Data.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeBytesSize(Data);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(_Request other) {
      if (other == null) {
        return;
      }
      if (other.Token.Length != 0) {
        Token = other.Token;
      }
      if (other.Rpc.Length != 0) {
        Rpc = other.Rpc;
      }
      if (other.Data.Length != 0) {
        Data = other.Data;
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
            Token = input.ReadString();
            break;
          }
          case 18: {
            Rpc = input.ReadString();
            break;
          }
          case 26: {
            Data = input.ReadBytes();
            break;
          }
        }
      }
    }

  }

  public sealed partial class _Response : pb::IMessage<_Response> {
    private static readonly pb::MessageParser<_Response> _parser = new pb::MessageParser<_Response>(() => new _Response());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<_Response> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Violet.Rpc.ClientReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public _Response() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public _Response(_Response other) : this() {
      token_ = other.token_;
      code_ = other.code_;
      rpc_ = other.rpc_;
      data_ = other.data_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public _Response Clone() {
      return new _Response(this);
    }

    /// <summary>Field number for the "token" field.</summary>
    public const int TokenFieldNumber = 1;
    private string token_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Token {
      get { return token_; }
      set {
        token_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "code" field.</summary>
    public const int CodeFieldNumber = 2;
    private int code_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Code {
      get { return code_; }
      set {
        code_ = value;
      }
    }

    /// <summary>Field number for the "rpc" field.</summary>
    public const int RpcFieldNumber = 3;
    private string rpc_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Rpc {
      get { return rpc_; }
      set {
        rpc_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "data" field.</summary>
    public const int DataFieldNumber = 4;
    private pb::ByteString data_ = pb::ByteString.Empty;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pb::ByteString Data {
      get { return data_; }
      set {
        data_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as _Response);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(_Response other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Token != other.Token) return false;
      if (Code != other.Code) return false;
      if (Rpc != other.Rpc) return false;
      if (Data != other.Data) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Token.Length != 0) hash ^= Token.GetHashCode();
      if (Code != 0) hash ^= Code.GetHashCode();
      if (Rpc.Length != 0) hash ^= Rpc.GetHashCode();
      if (Data.Length != 0) hash ^= Data.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Token.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Token);
      }
      if (Code != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(Code);
      }
      if (Rpc.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(Rpc);
      }
      if (Data.Length != 0) {
        output.WriteRawTag(34);
        output.WriteBytes(Data);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Token.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Token);
      }
      if (Code != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Code);
      }
      if (Rpc.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Rpc);
      }
      if (Data.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeBytesSize(Data);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(_Response other) {
      if (other == null) {
        return;
      }
      if (other.Token.Length != 0) {
        Token = other.Token;
      }
      if (other.Code != 0) {
        Code = other.Code;
      }
      if (other.Rpc.Length != 0) {
        Rpc = other.Rpc;
      }
      if (other.Data.Length != 0) {
        Data = other.Data;
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
            Token = input.ReadString();
            break;
          }
          case 16: {
            Code = input.ReadInt32();
            break;
          }
          case 26: {
            Rpc = input.ReadString();
            break;
          }
          case 34: {
            Data = input.ReadBytes();
            break;
          }
        }
      }
    }

  }

  public sealed partial class HelloRequest : pb::IMessage<HelloRequest> {
    private static readonly pb::MessageParser<HelloRequest> _parser = new pb::MessageParser<HelloRequest>(() => new HelloRequest());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<HelloRequest> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Violet.Rpc.ClientReflection.Descriptor.MessageTypes[2]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public HelloRequest() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public HelloRequest(HelloRequest other) : this() {
      content_ = other.content_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public HelloRequest Clone() {
      return new HelloRequest(this);
    }

    /// <summary>Field number for the "content" field.</summary>
    public const int ContentFieldNumber = 1;
    private string content_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Content {
      get { return content_; }
      set {
        content_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as HelloRequest);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(HelloRequest other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Content != other.Content) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Content.Length != 0) hash ^= Content.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Content.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Content);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Content.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Content);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(HelloRequest other) {
      if (other == null) {
        return;
      }
      if (other.Content.Length != 0) {
        Content = other.Content;
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
            Content = input.ReadString();
            break;
          }
        }
      }
    }

  }

  public sealed partial class HelloResponse : pb::IMessage<HelloResponse> {
    private static readonly pb::MessageParser<HelloResponse> _parser = new pb::MessageParser<HelloResponse>(() => new HelloResponse());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<HelloResponse> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Violet.Rpc.ClientReflection.Descriptor.MessageTypes[3]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public HelloResponse() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public HelloResponse(HelloResponse other) : this() {
      greet_ = other.greet_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public HelloResponse Clone() {
      return new HelloResponse(this);
    }

    /// <summary>Field number for the "greet" field.</summary>
    public const int GreetFieldNumber = 1;
    private string greet_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Greet {
      get { return greet_; }
      set {
        greet_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as HelloResponse);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(HelloResponse other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Greet != other.Greet) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Greet.Length != 0) hash ^= Greet.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Greet.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Greet);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Greet.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Greet);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(HelloResponse other) {
      if (other == null) {
        return;
      }
      if (other.Greet.Length != 0) {
        Greet = other.Greet;
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
            Greet = input.ReadString();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code