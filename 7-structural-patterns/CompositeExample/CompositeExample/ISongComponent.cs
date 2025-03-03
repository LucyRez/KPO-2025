using System;

namespace CompositeExample;

public interface ISongComponent
{
    void Play();

    void Add(ISongComponent songComponent) { throw new NotImplementedException(); }
}
