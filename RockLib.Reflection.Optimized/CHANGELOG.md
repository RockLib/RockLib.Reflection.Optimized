# RockLib.Reflection.Optimized Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## 1.3.1

#### Added

- Adds SourceLink to nuget package.

----

**Note:** Release notes in the above format are not available for earlier versions of
RockLib.Secrets. What follows below are the original release notes.

----

## 1.3.0

Adds net5.0 target and removes shared project styling

## 1.2.1

Widens the criteria for a class being a decorator:

- Removes the "must have either a public constructor with a parameter of type target interface or a public property of type target interface with a public setter" constraint.
- Includes base types when searching for instance fields of type target interface.

## 1.2.0

Adds Undecorate extension method.

## 1.1.2

Adds icon to project and nuget package.

## 1.1.1

Updates to align with nuget conventions.

## 1.1.0

Adds extension methods for getting and setting fields.

## 1.0.1

Adds support for .NET Framework 4.5.1.

## 1.0.0

Initial release. Includes extension methods for optimizing read/write
access to properties.
