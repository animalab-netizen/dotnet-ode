# UseCase Guide

Use `UseCase<TParam, TResult>` when one request should produce one explicit delivery.

Use `ChainUseCase<TParam, TIntermediate, TResult>` when a first successful result must contextualize a second step.

Use `SequenceUseCase<TParam, TResult>` when three or more ordered entries should be preserved from request to delivery.

Use `GuardAsync` when invalid dispatch must be rejected before I/O or state mutation.
