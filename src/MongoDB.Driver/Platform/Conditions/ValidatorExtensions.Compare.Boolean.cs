﻿#region Copyright (c) 2009 S. van Deursen
/* The MongoDB.Driver.Platform.Conditions library enables developers to validate pre- and postconditions in a fluent 
 * manner.
 * 
 * To contact me, please visit my blog at http://www.cuttingedge.it/blogs/steven/ 
 *
 * Copyright (c) 2009 S. van Deursen
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
 * associated documentation files (the "Software"), to deal in the Software without restriction, including
 * without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the
 * following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all copies or substantial
 * portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT
 * LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO
 * EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
 * IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE
 * USE OR OTHER DEALINGS IN THE SOFTWARE.
*/
#endregion

using System;

namespace MongoDB.Driver.Platform.Conditions
{
    // Comparable checks for Boolean
    public static partial class ValidatorExtensions
    {
        /// <summary>
        /// Checks whether the given value is <b>true</b>. An exception is thrown otherwise.
        /// </summary>
        /// <param name="validator">The <see cref="ConditionValidator{T}"/> that holds the value that has to be checked.</param>
        /// <returns>The specified <paramref name="validator"/> instance.</returns>
        /// <exception cref="ArgumentException">Thrown when the <see cref="ConditionValidator{T}.Value">Value</see> of the specified <paramref name="validator"/> is <b>false</b>, while the specified <paramref name="validator"/> is created using the <see cref="Condition.Requires{T}(T,string)">Requires</see> extension method.</exception>
        /// <exception cref="PostconditionException">Thrown when the <see cref="ConditionValidator{T}.Value">Value</see> of the specified <paramref name="validator"/> is <b>false</b>, while the specified <paramref name="validator"/> is created using the <see cref="Condition.Ensures{T}(T,string)">Ensures</see> extension method.</exception>
        public static ConditionValidator<bool> IsTrue(this ConditionValidator<bool> validator)
        {
            if (!validator.Value)
            {
                Throw.ValueShouldBeTrue(validator, null);
            }

            return validator;
        }

        /// <summary>
        /// Checks whether the given value is <b>true</b>. An exception is thrown otherwise.
        /// </summary>
        /// <param name="validator">The <see cref="ConditionValidator{T}"/> that holds the value that has to be checked.</param>
        /// <param name="conditionDescription">
        /// The description of the condition that should hold. The string may hold the placeholder '{0}' for 
        /// the <see cref="ConditionValidator{T}.ArgumentName">ArgumentName</see>.
        /// </param>
        /// <returns>The specified <paramref name="validator"/> instance.</returns>
        /// <exception cref="ArgumentException">Thrown when the <see cref="ConditionValidator{T}.Value">Value</see> of the specified <paramref name="validator"/> is <b>false</b>, while the specified <paramref name="validator"/> is created using the <see cref="Condition.Requires{T}(T,string)">Requires</see> extension method.</exception>
        /// <exception cref="PostconditionException">Thrown when the <see cref="ConditionValidator{T}.Value">Value</see> of the specified <paramref name="validator"/> is <b>false</b>, while the specified <paramref name="validator"/> is created using the <see cref="Condition.Ensures{T}(T,string)">Ensures</see> extension method.</exception>
        public static ConditionValidator<bool> IsTrue(this ConditionValidator<bool> validator, string conditionDescription)
        {
            if (!validator.Value)
            {
                Throw.ValueShouldBeTrue(validator, conditionDescription);
            }

            return validator;
        }

        /// <summary>
        /// Checks whether the given value is <b>true</b>. An exception is thrown otherwise.
        /// </summary>
        /// <param name="validator">The <see cref="ConditionValidator{T}"/> that holds the value that has to be checked.</param>
        /// <returns>The specified <paramref name="validator"/> instance.</returns>
        /// <exception cref="ArgumentException">Thrown when the <see cref="ConditionValidator{T}.Value">Value</see> of the specified <paramref name="validator"/> is <b>false</b> or null, while the specified <paramref name="validator"/> is created using the <see cref="Condition.Requires{T}(T,string)">Requires</see> extension method.</exception>
        /// <exception cref="PostconditionException">Thrown when the <see cref="ConditionValidator{T}.Value">Value</see> of the specified <paramref name="validator"/> is <b>false</b> or null, while the specified <paramref name="validator"/> is created using the <see cref="Condition.Ensures{T}(T,string)">Ensures</see> extension method.</exception>
        public static ConditionValidator<bool?> IsTrue(this ConditionValidator<bool?> validator)
        {
            if (!(validator.Value == true))
            {
                Throw.ValueShouldBeTrue(validator, null);
            }

            return validator;
        }

        /// <summary>
        /// Checks whether the given value is <b>true</b>. An exception is thrown otherwise.
        /// </summary>
        /// <param name="validator">The <see cref="ConditionValidator{T}"/> that holds the value that has to be checked.</param>
        /// <param name="conditionDescription">
        /// The description of the condition that should hold. The string may hold the placeholder '{0}' for 
        /// the <see cref="ConditionValidator{T}.ArgumentName">ArgumentName</see>.
        /// </param>
        /// <returns>The specified <paramref name="validator"/> instance.</returns>
        /// <exception cref="ArgumentException">Thrown when the <see cref="ConditionValidator{T}.Value">Value</see> of the specified <paramref name="validator"/> is <b>false</b> or null, while the specified <paramref name="validator"/> is created using the <see cref="Condition.Requires{T}(T,string)">Requires</see> extension method.</exception>
        /// <exception cref="PostconditionException">Thrown when the <see cref="ConditionValidator{T}.Value">Value</see> of the specified <paramref name="validator"/> is <b>false</b> or null, while the specified <paramref name="validator"/> is created using the <see cref="Condition.Ensures{T}(T,string)">Ensures</see> extension method.</exception>
        public static ConditionValidator<bool?> IsTrue(this ConditionValidator<bool?> validator, string conditionDescription)
        {
            if (!(validator.Value == true))
            {
                Throw.ValueShouldBeTrue(validator, conditionDescription);
            }

            return validator;
        }

        /// <summary>
        /// Checks whether the given value is <b>false</b>. An exception is thrown otherwise.
        /// </summary>
        /// <param name="validator">The <see cref="ConditionValidator{T}"/> that holds the value that has to be checked.</param>
        /// <returns>The specified <paramref name="validator"/> instance.</returns>
        /// <exception cref="ArgumentException">Thrown when the <see cref="ConditionValidator{T}.Value">Value</see> of the specified <paramref name="validator"/> is <b>true</b>, while the specified <paramref name="validator"/> is created using the <see cref="Condition.Requires{T}(T,string)">Requires</see> extension method.</exception>
        /// <exception cref="PostconditionException">Thrown when the <see cref="ConditionValidator{T}.Value">Value</see> of the specified <paramref name="validator"/> is <b>true</b>, while the specified <paramref name="validator"/> is created using the <see cref="Condition.Ensures{T}(T,string)">Ensures</see> extension method.</exception>
        public static ConditionValidator<bool> IsFalse(this ConditionValidator<bool> validator)
        {
            if (validator.Value)
            {
                Throw.ValueShouldBeFalse(validator, null);
            }

            return validator;
        }

        /// <summary>
        /// Checks whether the given value is <b>false</b>. An exception is thrown otherwise.
        /// </summary>
        /// <param name="validator">The <see cref="ConditionValidator{T}"/> that holds the value that has to be checked.</param>
        /// <param name="conditionDescription">
        /// The description of the condition that should hold. The string may hold the placeholder '{0}' for 
        /// the <see cref="ConditionValidator{T}.ArgumentName">ArgumentName</see>.
        /// </param>
        /// <returns>The specified <paramref name="validator"/> instance.</returns>
        /// <exception cref="ArgumentException">Thrown when the <see cref="ConditionValidator{T}.Value">Value</see> of the specified <paramref name="validator"/> is <b>true</b>, while the specified <paramref name="validator"/> is created using the <see cref="Condition.Requires{T}(T,string)">Requires</see> extension method.</exception>
        /// <exception cref="PostconditionException">Thrown when the <see cref="ConditionValidator{T}.Value">Value</see> of the specified <paramref name="validator"/> is <b>true</b>, while the specified <paramref name="validator"/> is created using the <see cref="Condition.Ensures{T}(T,string)">Ensures</see> extension method.</exception>
        public static ConditionValidator<bool> IsFalse(this ConditionValidator<bool> validator, string conditionDescription)
        {
            if (validator.Value)
            {
                Throw.ValueShouldBeFalse(validator, conditionDescription);
            }

            return validator;
        }

        /// <summary>
        /// Checks whether the given value is <b>false</b>. An exception is thrown otherwise.
        /// </summary>
        /// <param name="validator">The <see cref="ConditionValidator{T}"/> that holds the value that has to be checked.</param>
        /// <returns>The specified <paramref name="validator"/> instance.</returns>
        /// <exception cref="ArgumentException">Thrown when the <see cref="ConditionValidator{T}.Value">Value</see> of the specified <paramref name="validator"/> is <b>true</b> or null, while the specified <paramref name="validator"/> is created using the <see cref="Condition.Requires{T}(T,string)">Requires</see> extension method.</exception>
        /// <exception cref="PostconditionException">Thrown when the <see cref="ConditionValidator{T}.Value">Value</see> of the specified <paramref name="validator"/> is <b>true</b> or null, while the specified <paramref name="validator"/> is created using the <see cref="Condition.Ensures{T}(T,string)">Ensures</see> extension method.</exception>
        public static ConditionValidator<bool?> IsFalse(this ConditionValidator<bool?> validator)
        {
            if (!(validator.Value == false))
            {
                Throw.ValueShouldBeFalse(validator, null);
            }

            return validator;
        }

        /// <summary>
        /// Checks whether the given value is <b>false</b>. An exception is thrown otherwise.
        /// </summary>
        /// <param name="validator">The <see cref="ConditionValidator{T}"/> that holds the value that has to be checked.</param>
        /// <param name="conditionDescription">
        /// The description of the condition that should hold. The string may hold the placeholder '{0}' for 
        /// the <see cref="ConditionValidator{T}.ArgumentName">ArgumentName</see>.
        /// </param>
        /// <returns>The specified <paramref name="validator"/> instance.</returns>
        /// <exception cref="ArgumentException">Thrown when the <see cref="ConditionValidator{T}.Value">Value</see> of the specified <paramref name="validator"/> is <b>true</b> or null, while the specified <paramref name="validator"/> is created using the <see cref="Condition.Requires{T}(T,string)">Requires</see> extension method.</exception>
        /// <exception cref="PostconditionException">Thrown when the <see cref="ConditionValidator{T}.Value">Value</see> of the specified <paramref name="validator"/> is <b>true</b> or null, while the specified <paramref name="validator"/> is created using the <see cref="Condition.Ensures{T}(T,string)">Ensures</see> extension method.</exception>
        public static ConditionValidator<bool?> IsFalse(this ConditionValidator<bool?> validator, string conditionDescription)
        {
            if (!(validator.Value == false))
            {
                Throw.ValueShouldBeFalse(validator, conditionDescription);
            }

            return validator;
        }
    }
}