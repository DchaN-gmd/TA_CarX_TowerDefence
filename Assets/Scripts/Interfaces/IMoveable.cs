﻿using UnityEngine;

public interface IMoveable
{
    public void Move(Transform selfTransform, Transform target, float speed);
}