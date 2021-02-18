using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    public interface ISpacesDAO
    {

        IList<SpacesDAO> ListSpacesByID(VenueDAO venue);

        IList<SpacesDAO> List5SpacesByRequirements(DateTime daysNeeded, int guestsAttending);

        void BookReservaton(DateTime daysNeeded, SpacesDAO spaceBooked);


    }
}
