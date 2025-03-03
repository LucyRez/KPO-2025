using System;

namespace UniversalCarShop.Engines;

/// <summary>
/// Структура для случая, когда у двигателя нет каких-либо параметров
/// </summary>
public struct EmptyEngineParams
{
    public static readonly EmptyEngineParams DEFAULT = new EmptyEngineParams(); // Публичное статическое поле, доступное только для чтения, чтобы каждый раз не писать new
}