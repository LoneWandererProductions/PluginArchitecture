/*
 * COPYRIGHT:   See COPYING in the top level directory
 * PROJECT:     UnknownNamespace
 * FILE:        MSTestSettings.cs
 * PURPOSE:     Test settings for MSTest, configuring parallel execution and other test behaviors to optimize test runs and ensure reliable results.
 * PROGRAMMER:  Peter Geinitz (Wayfarer)
 */

[assembly: Parallelize(Scope = ExecutionScope.MethodLevel)]