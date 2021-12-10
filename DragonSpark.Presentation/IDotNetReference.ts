export interface IDotNetReference {
	invokeMethodAsync<T>(methodName: string, ...args: any[]): Promise<T>;
}