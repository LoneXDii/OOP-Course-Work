using Client.Persistence.Repositories;
using System.Globalization;

namespace Client.ValueConverters;

public class SenderIdToMessageAlignmentValueConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var _unitOfWork = Application.Current.Handler.MauiContext
                                                     .Services
                                                     .GetService<IUnitOfWork>();
        if (value is null || _unitOfWork is null)
            return LayoutOptions.Start;
        if((int)value != _unitOfWork.User.GetUser().Id)
        {
            return LayoutOptions.Start;
        }
        return LayoutOptions.End;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
