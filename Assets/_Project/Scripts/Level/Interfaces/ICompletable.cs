using System;

public interface ICompletable
{
    event Action Completed;
}
