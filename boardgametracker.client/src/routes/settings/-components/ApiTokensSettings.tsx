import type { ColumnDef } from "@tanstack/react-table";
import { useMemo, useState } from "react";
import { useTranslation } from "react-i18next";
import BgtButton from "@/components/BgtButton/BgtButton";
import { BgtLoadingSpinner } from "@/components/BgtLoadingSpinner/BgtLoadingSpinner";
import { BgtDataTable } from "@/components/BgtTable/BgtDataTable";
import { useModalState } from "@/hooks/useModalState";
import type { ApiToken, ApiTokenCreated } from "@/models";
import { BgtDeleteModal } from "@/routes/-modals/BgtDeleteModal";
import { useApiTokenData } from "../-hooks/useApiTokenData";
import { CreateApiTokenModal } from "../-modals/CreateApiTokenModal";
import { NewApiTokenModal } from "../-modals/NewApiTokenModal";
import { SettingsSection } from "./SettingsSection";

export const ApiTokensSettings = () => {
	const { t } = useTranslation(["settings", "common"]);
	const { tokens, isLoading, createToken, isCreating, revokeToken } = useApiTokenData();

	const createModal = useModalState();
	const [newToken, setNewToken] = useState<ApiTokenCreated | null>(null);
	const [revokeTarget, setRevokeTarget] = useState<ApiToken | null>(null);

	const columns: ColumnDef<ApiToken>[] = useMemo(
		() => [
			{
				accessorKey: "name",
				header: t("api-tokens.name"),
				cell: ({ row }) => (
					<div className={row.original.isRevoked ? "text-white/40 line-through" : ""}>{row.original.name}</div>
				),
			},
			{
				accessorKey: "createdAt",
				header: () => <div className="hidden md:block">{t("api-tokens.created")}</div>,
				cell: ({ row }) => (
					<div className="text-white/70 hidden md:block">{new Date(row.original.createdAt).toLocaleDateString()}</div>
				),
			},
			{
				accessorKey: "lastUsedAt",
				header: () => <div className="hidden md:block">{t("api-tokens.last-used")}</div>,
				cell: ({ row }) => (
					<div className="text-white/70 hidden md:block">
						{row.original.lastUsedAt
							? new Date(row.original.lastUsedAt).toLocaleDateString()
							: t("api-tokens.never-used")}
					</div>
				),
			},
			{
				id: "actions",
				header: "",
				cell: ({ row }) =>
					row.original.isRevoked ? (
						<div className="flex justify-end">
							<span className="text-white/40 text-sm">{t("api-tokens.revoked")}</span>
						</div>
					) : (
						<div className="flex justify-end">
							<BgtButton size="1" variant="error" onClick={() => setRevokeTarget(row.original)}>
								{t("api-tokens.revoke.button")}
							</BgtButton>
						</div>
					),
			},
		],
		[t],
	);

	if (isLoading) {
		return (
			<div className="flex items-center justify-center py-8">
				<BgtLoadingSpinner />
			</div>
		);
	}

	return (
		<>
			<SettingsSection title={t("api-tokens.title")} description={t("api-tokens.description")}>
				<div className="mb-4">
					<BgtButton onClick={createModal.show}>{t("api-tokens.create-button")}</BgtButton>
				</div>
				<BgtDataTable columns={columns} data={tokens} noDataMessage={t("api-tokens.no-tokens")} />
			</SettingsSection>

			{createModal.isOpen && (
				<CreateApiTokenModal
					open={createModal.isOpen}
					close={createModal.hide}
					onSubmit={createToken}
					onCreated={setNewToken}
					isLoading={isCreating}
				/>
			)}

			{newToken && (
				<NewApiTokenModal open={true} close={() => setNewToken(null)} name={newToken.name} token={newToken.token} />
			)}

			{revokeTarget && (
				<BgtDeleteModal
					open={true}
					close={() => setRevokeTarget(null)}
					onDelete={async () => {
						await revokeToken(revokeTarget.id);
						setRevokeTarget(null);
					}}
					title={revokeTarget.name}
					description={t("api-tokens.revoke.description", { name: revokeTarget.name })}
				/>
			)}
		</>
	);
};
