using System;

namespace UniversalCarShop.UseCases.Engines;

/// <summary>
/// Структура для случая, когда у двигателя нет каких-либо параметров
/// </summary>
public struct HandEngineParams
{
    public static readonly HandEngineParams DEFAULT = new HandEngineParams(); // Публичное статическое поле, доступное только для чтения, чтобы каждый раз не писать new
}