export interface ApiToken {
	id: number;
	name: string;
	createdAt: string;
	lastUsedAt: string | null;
	isRevoked: boolean;
}

export interface ApiTokenCreated {
	id: number;
	name: string;
	token: string;
	createdAt: string;
}
