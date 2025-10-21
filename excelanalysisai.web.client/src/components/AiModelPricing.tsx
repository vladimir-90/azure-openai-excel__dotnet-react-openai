import type { AIModelDto } from '../services/generation-settings.service';

interface AiModelPricingProps {
	selectedModel: AIModelDto | undefined;
}

function AiModelPricing({ selectedModel }: AiModelPricingProps) {
	const formatPrice = (price: number): string => {
		return `$${price.toFixed(2)}`;
	};

	if (!selectedModel) {
		return null;
	}

	return (
		<div className="mt-2 p-3 bg-light rounded-3 border">
			<div className="row text-center">
				<div className="col-12 mb-2">
					<small className="text-muted fw-semibold">
						💰 Pricing (per 1M tokens)
					</small>
				</div>
				<div className="col-4">
					<div className="text-success fw-bold">
						{formatPrice(selectedModel.pricing.input)}
					</div>
					<small className="text-muted">Input</small>
				</div>
				<div className="col-4">
					<div className="text-info fw-bold">
						{formatPrice(selectedModel.pricing.cachedInput)}
					</div>
					<small className="text-muted">Cached</small>
				</div>
				<div className="col-4">
					<div className="text-warning fw-bold">
						{formatPrice(selectedModel.pricing.output)}
					</div>
					<small className="text-muted">Output</small>
				</div>
			</div>
		</div>
	);
}

export default AiModelPricing;
