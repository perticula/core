// perticula - model - ISerializationrService.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace model.Service;

public interface ISerializationService<T> : ITransientService
{
	string Serialize(T obj);
	string Serialize(T obj, Type type);

	T Deserialize(string text);
}
