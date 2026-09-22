import { useForm } from "@tanstack/react-form";
import { useState } from "react";
import { useTranslation } from "react-i18next";
import BgtButton from "@/components/BgtButton/BgtButton";
import {
	BgtDialog,
	BgtDialogClose,
	BgtDialogContent,
	BgtDialogDescription,
	BgtDialogTitle,
} from "@/components/BgtDialog";
import { BgtInputField } from "@/components/BgtForm";
import { type ApiTokenCreated, isApiError } from "@/models";
import { translateApiError } from "@/utils/errorUtils";
import { handleFormSubmit } from "@/utils/formUtils";

interface Props {
	open: boolean;
	close: () => void;
	onSubmit: (name: string) => Promise<ApiTokenCreated>;
	onCreated: (token: ApiTokenCreated) => void;
	isLoading: boolean;
}

export const CreateApiTokenModal = ({ open, close, onSubmit, onCreated, isLoading }: Props) => {
	const { t } = useTranslation(["settings", "common"]);
	const [error, setError] = useState<string | null>(null);

	const form = useForm({
		defaultValues: { name: "" },
		onSubmit: async ({ value }) => {
			setError(null);
			try {
				const created = await onSubmit(value.name);
				close();
				onCreated(created);
			} catch (e) {
				setError(
					isApiError(e)
						? translateApiError(e.message, "settings:api-tokens.notifications.create-failed")
						: t("api-tokens.notifications.create-failed"),
				);
			}
		},
	});

	return (
		<BgtDialog open={open} onClose={close}>
			<BgtDialogContent>
				<form onSubmit={handleFormSubmit(form)} className="w-full">
					<BgtDialogTitle>{t("api-tokens.create.title")}</BgtDialogTitle>
					<BgtDialogDescription>{t("api-tokens.create.description")}</BgtDialogDescription>
					<div className="flex flex-col gap-2 mb-3 mt-3">
						<form.Field
							name="name"
							validators={{
								onChange: ({ value }) => {
									if (!value) return t("common:required", "Required");
									return undefined;
								},
							}}
						>
							{(field) => (
								<BgtInputField
									field={field}
									type="text"
									label={t("api-tokens.create.name.label")}
									disabled={isLoading}
								/>
							)}
						</form.Field>
					</div>
					{error && <div className="text-error text-sm mb-2">{error}</div>}
					<BgtDialogClose>
						<BgtButton variant="cancel" onClick={close} disabled={isLoading}>
							{t("common:cancel")}
						</BgtButton>
						<BgtButton variant="primary" type="submit" disabled={isLoading}>
							{t("api-tokens.create.save")}
						</BgtButton>
					</BgtDialogClose>
				</form>
			</BgtDialogContent>
		</BgtDialog>
	);
};
