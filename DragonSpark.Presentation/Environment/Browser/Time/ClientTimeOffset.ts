export function GetClientTimeOffset(): number {
	return new Date().getTimezoneOffset() * -1;
}