/* Copyright 2013-present MongoDB Inc.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System;
#if NET45
using System.Runtime.Serialization;
#endif
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver.Core.Connections;
using MongoDB.Driver.Core.Misc;

namespace MongoDB.Driver
{
    /// <summary>
    /// Represents a MongoDB node is recovering exception.
    /// </summary>
#if NET45
    [Serializable]
#endif
    public class MongoNodeIsRecoveringException : MongoServerException
    {
        #region static
        // private static methods
        private static string CreateMessage(BsonDocument result)
        {
            var code = result.GetValue("code", -1).ToInt32();
            var codeName = result.GetValue("codeName", null)?.AsString;

            if (codeName == null)
            {
                return $"Server returned node is recovering error (code = {code}).";
            }
            else
            {
                return $"Server returned node is recovering error (code = {code}, codeName = \"{codeName}\").";
            }
        }
        #endregion

        // fields
        private readonly BsonDocument _result;

        // constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MongoNodeIsRecoveringException"/> class.
        /// </summary>
        /// <param name="connectionId">The connection identifier.</param>
        /// <param name="result">The result.</param>
        public MongoNodeIsRecoveringException(ConnectionId connectionId, BsonDocument result)
            : base(connectionId, CreateMessage(result))
        {
            _result = Ensure.IsNotNull(result, nameof(result));
        }

#if NET45
        /// <summary>
        /// Initializes a new instance of the <see cref="MongoNodeIsRecoveringException"/> class.
        /// </summary>
        /// <param name="info">The SerializationInfo.</param>
        /// <param name="context">The StreamingContext.</param>
        protected MongoNodeIsRecoveringException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _result = (BsonDocument)info.GetValue("_result", typeof(BsonDocument));
        }
#endif

        // properties
        /// <summary>
        /// Gets the result from the server.
        /// </summary>
        /// <value>
        /// The result from the server.
        /// </value>
        public BsonDocument Result
        {
            get { return _result; }
        }

        // methods
#if NET45
        /// <inheritdoc/>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("_result", _result);
        }
#endif
    }
}
